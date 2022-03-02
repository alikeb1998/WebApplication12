using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Reports;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Trades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.ExternalServices.Rest.IExternalService;

namespace Iz.Online.Services.Services
{
    public class TradeService : ITradeServices
    {
        public IExternalTradeService _externalTradeService { get; }
        public string Token { get; set; }

        public TradeService(IExternalTradeService externalTradeService)
        {
            _externalTradeService = externalTradeService;

        }
        

        public ResultModel<List<Trade>> Trades()
        {

            var trades = _externalTradeService.Trades();

            if (!trades.IsSuccess || trades.Model.Trades == null)
                return new ResultModel<List<Trade>>(null, trades.StatusCode == 200, trades.Message, trades.StatusCode);

            var allTrades = trades.Model.Trades.Where(t => t.TradedAt.ToString().Substring(0, 6) == DateTime.Today.ToString().Substring(0, 6)).Select(x => new Trade()
            {
                Name = x.Order.instrument.name,
                Price = x.Price,
                State = x.Order.state,
                OrderSide = x.Order.orderSide,
                ExecutedQ = (long)x.Order.executedQ,
                TradedAt = x.TradedAt
                  ,
                InstrumentId = x.Order.instrument.id,
                NscCode = x.Order.instrument.code
            }).ToList();

            return new ResultModel<List<Trade>>(allTrades);
        }

        public ResultModel<List<Trade>> TradesPaged(TradeFilter filter)
        {
            var trades = _externalTradeService.Trades();
            if (!trades.IsSuccess || trades.Model.Trades == null)
                return new ResultModel<List<Trade>>(null, trades.StatusCode == 200, trades.Message, trades.StatusCode);

            var allTrades = trades.Model.Trades.Where(t => t.TradedAt.ToString().Substring(0, 6) == DateTime.Today.ToString().Substring(0, 6)).Select(x => new Trade()
            {
                Name = x.Order.instrument.name,
                Price = x.Price,
                State = x.Order.state,
                OrderSide = x.Order.orderSide,
                ExecutedQ = (long)x.Order.executedQ,
                TradedAt = x.TradedAt
                  ,
                InstrumentId = x.Order.instrument.id,
                NscCode = x.Order.instrument.code
            }).ToList();

            var res = Filter(allTrades, filter);

            return new ResultModel<List<Trade>>(res);
        }

        public ResultModel<TradeHistoryReport> History(TradeHistoryFilter filter)
        {
            var list = _externalTradeService.Trades();
            var result = list.Model.Trades.Select(x => new Trade()
            {
                Price = x.Price,
                TradedAt = x.TradedAt,
                Name = x.Order.instrument.name,
                OrderSide = x.Order.orderSide,
                Quantity = x.Order.quantity,
                TradeValue = x.Order.executedQ * x.Order.price,
                TradeId = x.Order.id,
                State = x.Order.state
            }).ToList();

            var a = TradeHistoryFilter(result, filter);

            var res = new TradeHistoryReport()
            {
                Model = a,
                OrderType = filter.OrderType,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = result.Count,
            };
            return new ResultModel<TradeHistoryReport>(res);


        }
        private List<Trade> Filter(List<Trade> list, TradeFilter filter)
        {
            var report = new TradeReport()
            {
                Model = list.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).ToList(),
                TradeSortColumn = filter.TradeSortColumn,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = list.Count,
                OrderType = filter.OrderType,
            };

            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.TradeSortColumn)
                    {
                        case TradeSortColumn.Symbol:
                            return report.Model.OrderBy(x => x.Name).ToList();

                        case TradeSortColumn.Side:
                            return report.Model.OrderBy(x => x.OrderSide).ToList();

                        case TradeSortColumn.Date:
                            return report.Model.OrderBy(x => x.TradedAt).ToList();

                        case TradeSortColumn.Price:
                            return report.Model.OrderBy(x => x.Price).ToList();

                    }
                    break;

                case OrderType.DESC:
                    switch (filter.TradeSortColumn)
                    {
                        case TradeSortColumn.Symbol:
                            return report.Model.OrderByDescending(x => x.Name).ToList();

                        case TradeSortColumn.Side:
                            return report.Model.OrderByDescending(x => x.OrderSide).ToList();

                        case TradeSortColumn.Date:
                            return report.Model.OrderByDescending(x => x.TradedAt).ToList();

                        case TradeSortColumn.Price:
                            return report.Model.OrderByDescending(x => x.Price).ToList();

                    }
                    break;

                default:
                    return report.Model.OrderByDescending(x => x.TradedAt).ToList();
            }
            return report.Model.OrderByDescending(x => x.TradedAt).ToList();
        }
        private List<Trade> TradeHistoryFilter(List<Trade> list, TradeHistoryFilter filter)
        {


            if (filter.From == DateTime.MinValue)
            {
                var a = list.OrderBy(e => e.TradedAt).ToList();
                filter.From = a[0].TradedAt;
            }
            if (filter.To == DateTime.MinValue)
            {
                var a = list.OrderByDescending(e => e.TradedAt).ToList();
                filter.To = a[0].TradedAt;
            }


            list = list.Where
            (x => x.TradedAt - DateTime.MinValue >= filter.From - DateTime.MinValue && x.TradedAt - DateTime.MinValue <= filter.To - DateTime.MinValue)
            .OrderByDescending(x => x.TradedAt).ToList();

            if (filter.InstrumentId != 0)
            {
                list = list.Where(x => x.InstrumentId == filter.InstrumentId).ToList();
            }

            if (filter.OrderSide != 0)
            {
                switch (filter.OrderSide)
                {
                    case 1:
                        list = list.Where(x => x.OrderSide == 1).ToList();
                        break;

                    case 2:
                        list = list.Where(x => x.OrderSide == 2).ToList();
                        break;
                }
            }

            if (!string.IsNullOrEmpty(filter.State))
            {
                switch (filter.State)
                {
                    case "انجام شده":
                        list = list.Where(x => x.State == "انجام شده").ToList();
                        break;

                    case "منقضی شده":
                        list = list.Where(x => x.State == "منقضی شده").ToList();
                        break;
                    case "درحال انتظار":
                        list = list.Where(x => x.State == "درحال انتظار").ToList();
                        break;
                }
            }
            var report = new TradeHistoryReport()
            {
                Model = list.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).ToList(),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = list.Count,
                OrderType = filter.OrderType,
            };
            return report.Model.OrderBy(x => x.TradedAt).ToList();
        }
    }
}
