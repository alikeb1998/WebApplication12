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
using System.Net;
using CashHelper;

namespace Iz.Online.Services.Services
{
    public class TradeService : ITradeServices
    {
        public IExternalTradeService _externalTradeService { get; }
        public string Token { get; set; }
        private readonly ICacheService _cacheService;
        public TradeService(IExternalTradeService externalTradeService, ICacheService cacheService)
        {
            _externalTradeService = externalTradeService;
            _cacheService = cacheService;
        }


        public async Task<ResultModel<List<Trade>>> Trades()
        {
            var instruments = _cacheService.InstrumentData();

            var trades = await _externalTradeService.Trades();

            if (!trades.IsSuccess || trades.Model.Trades == null)
                return new ResultModel<List<Trade>>(null, trades.StatusCode == 200, trades.Message, trades.StatusCode);

            var allTrades = trades.Model.Trades.Where(t => t.TradedAt.ToString().Substring(0, 6) == DateTime.Today.ToString().Substring(0, 6)).Select(x => new Trade()
            {
                Name = x.Order.instrument.name,
                Price = x.Price,
                State = x.InternalState,
                OrderSide = x.Order.orderSide,
                ExecutedQ = (long)x.Order.executedQ,
                TradedAt = x.TradedAt,
                InstrumentId = _cacheService.GetLocalInstrumentIdFromOmsId(x.Order.instrument.id),
                StateText = x.InternalState switch { 1 => "لغو شده", 2 => "معامله شده", 3 => "معمله شده توسط ناظر بازار" }
                //NscCode = x.Order.instrument.code
            }).OrderByDescending(x=>x.TradedAt).ToList();


            return new ResultModel<List<Trade>>(allTrades);
        }

        public async Task<ResultModel<List<Trade>>> TradesPaged(TradeFilter filter)
        {
            if (filter.PageNumber == 0 || filter.PageSize == 0)
            {
                return new ResultModel<List<Trade>>(null, 400);
            }
            var trades = await _externalTradeService.Trades();
            if (!trades.IsSuccess || trades.Model.Trades == null)
                return new ResultModel<List<Trade>>(null, trades.StatusCode == 200, trades.Message, trades.StatusCode);

            var allTrades = trades.Model.Trades.Where(t => t.TradedAt.ToString().Substring(0, 6) == DateTime.Today.ToString().Substring(0, 6)).Select(x => new Trade()
            {
                Name = x.Order.instrument.name,
                Price = x.Price,
                State = x.InternalState,
                OrderSide = x.Order.orderSide,
                ExecutedQ = (long)x.Order.executedQ,
                TradedAt = x.TradedAt,
                InstrumentId = x.Order.instrument.id,
                NscCode = x.Order.instrument.code,
                //StateText = x.InternalState switch { }


            }).ToList();

            var res = Filter(allTrades, filter);

            return new ResultModel<List<Trade>>(res.OrderByDescending(x => x.TradedAt).ToList());
        }

        public async Task<ResultModel<TradeHistoryReport>> History(TradeHistoryFilter filter)
        {
            if (filter.PageNumber == 0 || filter.PageSize == 0)
            {
                return new ResultModel<TradeHistoryReport>(null, 400);
            }

            var list = await _externalTradeService.Trades();

            if (list.IsSuccess && list.Model.Trades != null)
            {

                var result = list.Model.Trades.Select(x => new Trade()
                {
                    Price = x.Price,
                    TradedAt = x.TradedAt,
                    Name = x.Order.instrument.name,
                    OrderSide = x.Order.orderSide,
                    Quantity = x.Order.quantity,
                    TradeValue = x.Order.executedQ * x.Order.price,
                    TradeId = x.Order.id,
                    State = x.InternalState,
                    InstrumentId = x.Order.instrument.id,
                    StateText = x.InternalState switch { 1 => "لغو شده", 2 => "معامله شده", 3 => "معمله شده توسط ناظر بازار" }

                }).OrderByDescending(x => x.TradedAt).ToList();

                var a = TradeHistoryFilter(result, filter);
                if (a != null)
                {
                    a.Model = a.Model.OrderByDescending(x => x.TradedAt).ToList();
                    return new ResultModel<TradeHistoryReport>(a);

                }


                return new ResultModel<TradeHistoryReport>(new TradeHistoryReport() { Model=new(),PageNumber = 0, PageSize = 0,TotalCount = 0}, 200, "لیست خالی است.");
            }
            return new ResultModel<TradeHistoryReport>(new TradeHistoryReport() { Model = new(), PageNumber = 0, PageSize = 0, TotalCount = 0 }, 200, "لیست خالی است.");
        }
        private List<Trade> Filter(List<Trade> list, TradeFilter filter)
        {
            //var report = new TradeReport()
            //{
            //    Model = list.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).ToList(),
            //    TradeSortColumn = filter.TradeSortColumn,
            //    PageNumber = filter.PageNumber,
            //    PageSize = filter.PageSize,
            //    TotalCount = list.Count,
            //    OrderType = filter.OrderType,
            //};

            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.TradeSortColumn)
                    {
                        case TradeSortColumn.Symbol:
                            return list = list.OrderBy(x => x.Name).ToList();

                        case TradeSortColumn.Side:
                            return list = list.OrderBy(x => x.OrderSide).ToList();

                        case TradeSortColumn.Date:
                            return list = list.OrderBy(x => x.TradedAt).ToList();

                        case TradeSortColumn.Price:
                            return list = list.OrderBy(x => x.Price).ToList();

                    }
                    break;

                case OrderType.DESC:
                    switch (filter.TradeSortColumn)
                    {

                        case TradeSortColumn.Date:
                            return list = list.OrderByDescending(x => x.TradedAt).ToList();
                        case TradeSortColumn.Side:
                            return list = list.OrderByDescending(x => x.OrderSide).ToList();

                        case TradeSortColumn.Symbol:
                            return list = list.OrderByDescending(x => x.Name).ToList();
                        case TradeSortColumn.Price:
                            return list = list.OrderByDescending(x => x.Price).ToList();

                    }
                    break;

                default:
                    return list = list.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).OrderByDescending(x => x.TradedAt).ToList();
            }

            return list = list.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).OrderByDescending(x => x.TradedAt).ToList();
        }
        private TradeHistoryReport TradeHistoryFilter(List<Trade> list, TradeHistoryFilter filter)
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


            //list = list.Where
            //(x => x.TradedAt - DateTime.MinValue >= filter.From - DateTime.MinValue && x.TradedAt - DateTime.MinValue <= filter.To - DateTime.MinValue)
            //.OrderByDescending(x => x.TradedAt).ToList();

            list = list.Where(x => DateTime.Compare(x.TradedAt, filter.From) >= 0 && DateTime.Compare(filter.To, x.TradedAt) >= 0).ToList();

            var tradeList = new List<Trade>();
            if (filter.InstrumentId.Count == 0)
            {
                tradeList.AddRange(list);
            }
            foreach (var f in filter.InstrumentId)
            {
                if (filter.InstrumentId.Count == 1 && f == 0)
                {
                    tradeList.AddRange(list);
                    //var a = list.Where(x => x.InstrumentId == f).ToList();
                    //tradeList.AddRange(a);
                }
                if (f != 0)
                {
                    var a = list.Where(x => _cacheService.GetLocalInstrumentIdFromOmsId(x.InstrumentId) == f).ToList();
                    tradeList.AddRange(a);
                }
            }
            if (tradeList.Count == 0)
            {
                return null;
            }

            if (filter.OrderSide != 0)
            {
                switch (filter.OrderSide)
                {
                    case 1:
                        tradeList = tradeList.Where(x => x.OrderSide == 1).ToList();
                        break;

                    case 2:
                        tradeList = tradeList.Where(x => x.OrderSide == 2).ToList();
                        break;
                }
            }


            if (filter.State != 0)
            {
                switch (filter.State)
                {
                    case 1:
                        tradeList = tradeList.Where(x => x.State == 1).ToList();
                        break;  
                    case 2:
                        tradeList = tradeList.Where(x => x.State == 2).ToList();
                        break;
                    case 3:
                        tradeList = tradeList.Where(x => x.State == 3).ToList();
                        break;
                }
            }
            var report = new TradeHistoryReport()
            {
                Model = tradeList.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).ToList(),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = list.Count,
                OrderType = filter.OrderType,
            };
            return report;
        }
    }
}
