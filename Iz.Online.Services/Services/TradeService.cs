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

namespace Iz.Online.Services.Services
{
    public  class TradeService : ITradeServices
    {
        private readonly IExternalTradeService _externalTradeService;

        public TradeService(IExternalTradeService externalTradeService)
        {
            _externalTradeService = externalTradeService;
        }

        public ResultModel<List<Trade>> Trades()
        {
            var trades = _externalTradeService.Trades();

            if (!trades.IsSuccess)
                return new ResultModel<List<Trade>>(null, false, trades.Message, trades.StatusCode);
            
            var allTrades = trades.Model.Trades.Where(t => t.TradedAt.ToString().Substring(0,6) == DateTime.Today.ToString().Substring(0,6)).Select(x => new Trade()
            {
                Name = x.Order.instrument.name,
                Price = x.Price,
                State = Convert.ToInt32(x.Order.state),
                OrderSide = x.Order.orderSide,
                ExecutedQ = (long)x.Order.executedQ,
                TradedAt = x.TradedAt
                ,InstrumentId= x.Order.instrument.id,
                NscCode = x.Order.instrument.code
            }).ToList();

            return new ResultModel<List<Trade>>(allTrades);
        }

        public ResultModel<List<Trade>> TradesPaged(TradeFilter filter)
        {
            var trades = _externalTradeService.Trades();

            if (!trades.IsSuccess)
                return new ResultModel<List<Trade>>(null, false, trades.Message, trades.StatusCode);

            var allTrades = trades.Model.Trades.Where(t => t.TradedAt.ToString().Substring(0, 6) == DateTime.Today.ToString().Substring(0, 6)).Select(x => new Trade()
            {
                Name = x.Order.instrument.name,
                Price = x.Price,
                State = Convert.ToInt32(x.Order.state),
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
        public List<Trade> Filter(List<Trade> list, TradeFilter filter)
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
    }
}
