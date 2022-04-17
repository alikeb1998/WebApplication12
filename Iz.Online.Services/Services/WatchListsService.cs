using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.IExternalService;
using Iz.Online.HubHandler;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Services.Services
{
    public class WatchListsService : IWatchListsService
    {
        private readonly IWatchListsRepository _watchListsRepository;
        private readonly IExternalInstrumentService _externalInstrumentsService;
        private readonly ICacheService _cache;

        public IExternalInstrumentService _externalInstrumentsServices { get; }
        public IExternalOrderService _externalOrderService { get; }

        public WatchListsService(IWatchListsRepository watchListsRepository, IExternalInstrumentService externalInstrumentsService, IExternalOrderService externalOrderService, ICacheService cacheService)
        {
            _watchListsRepository = watchListsRepository;
            _externalInstrumentsService = externalInstrumentsService;
            _cache = cacheService;
            _externalInstrumentsServices = externalInstrumentsService;
            _externalOrderService = externalOrderService;
        }

        public async Task<ResultModel<WatchListDetails>> WatchListDetails(SearchWatchList model)
        {
            var wl = await _watchListsRepository.GetWatchListDetails(model);


            foreach (var ins in wl.Model.Instruments)
            {
                //TODO:Implementsignal r in watchlist
                var instrumentDetails =  _cache.InstrumentData((int)ins.Id);
                var bestLimit = await _externalInstrumentsService.BestLimits(instrumentDetails.NscCode, instrumentDetails.InstrumentId,"");
                var price = await _externalInstrumentsService.Price(instrumentDetails.NscCode);

                if (!(bestLimit.IsSuccess && price.IsSuccess))
                    continue;

                if (bestLimit.Model == null || price.Model == null)
                    continue;

                ins.ClosePrice = price.Model.closingPrice.Value;
                ins.AskPrice = bestLimit.Model.orderRow1 == null ? 0 : bestLimit.Model.orderRow1.priceBestBuy;
                ins.BidPrice = bestLimit.Model.orderRow1 == null ? 0 : bestLimit.Model.orderRow1.priceBestSale;
                ins.BuyVolume = bestLimit.Model.orderRow1 == null ? 0 : bestLimit.Model.orderRow1.volumeBestBuy;
                ins.SellVolume = bestLimit.Model.orderRow1 == null ? 0 : bestLimit.Model.orderRow1.volumeBestSale;
                var lastPrice = price.Model.lastPrice.Value;
                var yesterdayPrice = price.Model.yesterdayPrice;

                if (yesterdayPrice > 0 && lastPrice > 0)
                    ins.LastPriceChangePercent = (float)((lastPrice - yesterdayPrice) / yesterdayPrice) * 100;

                ins.LastPrice = price.Model.lastPrice.Value;
                var name = ins.SymbolName;
                ins.SymbolName = name.EndsWith("1") ? name.Substring(0, ins.SymbolName.Length - 1) : name;
                ins.InstrumentId = instrumentDetails.Id;
            }

            return wl;
        }


        public async Task<ResultModel<List<WatchList>>> DeleteWatchList(SearchWatchList model)
        {
            return await _watchListsRepository.DeleteWatchList(model);
        }

        public async Task<ResultModel<WatchListDetails>> NewWatchList(NewWatchList model)
        {
            model.WatchListName = model.WatchListName.Trim();
            var res = await ValidateWatchList(model/*, out var resultModel*/);
            if (res.IsValidated)
                return res.Model;

            return await _watchListsRepository.NewWatchList(model);
        }

        private async Task<ValidatedWatchList> ValidateWatchList(NewWatchList model /*, out ResultModel<WatchListDetails> resultModel*/)
        {
            var maxLen = Convert.ToInt32(_cache.ConfigData("WatchListMaxLenName").Value);
            /////
            ResultModel<WatchListDetails> resultModel;
            /////
            var oldWl = await _watchListsRepository.GetUserWatchLists(model.TradingId);
            if (oldWl.IsSuccess)
            {
                if (oldWl.Model != null && oldWl.Model.Select(x => x.WatchListName).Contains(model.WatchListName))
                {
                    resultModel = new(null, false, "نام دیده بان تکراری است");
                    return new ValidatedWatchList() { IsValidated = true, Model = resultModel };
                }
            }

            if (model.Id.Count() > maxLen)
            {
                resultModel = new(null, false, "حداکثر تعداد نماد در دیده بان" + maxLen + "است");
                return new ValidatedWatchList() { IsValidated = true, Model = resultModel };
            }

            if (model.WatchListName.Length >
                Convert.ToInt32(_cache.ConfigData("WatchListMaxInstruments").Value))
            {
                resultModel = new(null, false, "محدودیت در طول حروف نام دیده بان");
                return new ValidatedWatchList() { IsValidated = true, Model = resultModel };
            }

            if (string.IsNullOrEmpty(model.TradingId))
            {
                resultModel = new(null, false, "مالک دیده بان مشخص نیست");
                return new ValidatedWatchList() { IsValidated = true, Model = resultModel };
            }

            if (string.IsNullOrEmpty(model.WatchListName))
            {
                resultModel = new(null, false, "نام دیده بان اجباری است");
                return new ValidatedWatchList() { IsValidated = true, Model = resultModel };
            }
            if (model.Id.Distinct().Count() != model.Id.Count())
            {
                resultModel = new(null, false, "وجود نماد تکراری در یک دیده بان");
                return new ValidatedWatchList() { IsValidated = true, Model = resultModel };
            }

            resultModel = new(null);
            return new ValidatedWatchList() { IsValidated = false, Model = resultModel };

        }
        private async Task<ValidatedWatchList> ValidateWatchList(EditWatchList model/*, out ResultModel<WatchListDetails> resultModel*/)
        {
            var maxLen = Convert.ToInt32(_cache.ConfigData("WatchListMaxLenName").Value);
            /////
            ResultModel<WatchListDetails> resultModel;
            /////
            var oldWl = await _watchListsRepository.GetUserWatchLists(model.TradingId);
            if (oldWl.IsSuccess)
                if (oldWl.Model.Where(x => x.Id != model.WatchListId).Select(x => x.WatchListName).Contains(model.WatchListName))
                {
                    resultModel = new(null, false, "نام دیده بان تکراری است");
                    return new ValidatedWatchList() { IsValidated = true, Model = resultModel };
                }

            var entity = await _watchListsRepository.GetWatchListDetails(new SearchWatchList()
            {
                TradingId = model.TradingId,
                WatchListId = model.WatchListId

            });

            if (!entity.IsSuccess)
            {
                resultModel = new(null, false, "دیده بان یافت نشد");
                return new ValidatedWatchList() { IsValidated = true, Model = resultModel };
            }
            if (model.Id.Count() > maxLen)
            {
                resultModel = new(null, false, "حداکثر تعداد نماد در دیده بان" + maxLen + "است");
                return new ValidatedWatchList() { IsValidated = true, Model = resultModel };
            }

            if (model.WatchListName.Length >
                Convert.ToInt32(_cache.ConfigData("WatchListMaxInstruments").Value))
            {
                resultModel = new(null, false, "محدودیت در طول حروف نام دیده بان");
                return new ValidatedWatchList() { IsValidated = true, Model = resultModel };
            }

            if (string.IsNullOrEmpty(model.TradingId))
            {
                resultModel = new(null, false, "مالک دیده بان مشخص نیست");
                return new ValidatedWatchList() { IsValidated = true, Model = resultModel };
            }

            if (string.IsNullOrEmpty(model.WatchListName))
            {
                resultModel = new(null, false, "نام دیده بان اجباری است");
                return new ValidatedWatchList() { IsValidated = true, Model = resultModel };
            }

            if (model.Id.Distinct().Count() != model.Id.Count())
            {
                resultModel = new(null, false, "وجود نماد تکراری در یک دیده بان");
                return new ValidatedWatchList() { IsValidated = true, Model = resultModel };
            }

            resultModel = new(null);
            return new ValidatedWatchList() { IsValidated = false, Model = resultModel };
        }

        public async Task<ResultModel<WatchListDetails>> AddInstrumentToWatchList(EditEathListItems model)
        {
            var maxLen = Convert.ToInt32(_cache.ConfigData("WatchListMaxInstruments").Value);

            var old = await _watchListsRepository.GetWatchListDetails(new SearchWatchList() { WatchListId = model.WatchListId });
            if (old.Model.Instruments.Count() >= maxLen)
                return new ResultModel<WatchListDetails>(null, false, "حداکثر تعداد نماد در دیده بان" + maxLen + "است");

            return await _watchListsRepository.AddInstrumentToWatchList(model);

        }

        public async Task<ResultModel<WatchListDetails>> RemoveInstrumentFromWatchList(EditEathListItems model)
        {
            return await _watchListsRepository.RemoveInstrumentFromWatchList(model);

        }

        public async Task<ResultModel<List<WatchList>>> InstrumentWatchLists(InstrumentWatchLists model)
        {

            return await _watchListsRepository.InstrumentWatchLists(model);

        }
        public async Task<ResultModel<WatchListDetails>> UpdateWatchList(EditWatchList model)
        {
            //var a = _cache.GetOmsIdFromLocalInstrumentId((int)model.Id[0]);

            model.WatchListName = model.WatchListName.Trim();
            var res = await ValidateWatchList(model);
            if (res.IsValidated)
                return res.Model;

            return await _watchListsRepository.UpdateWatchList(model);

        }



        public async Task<ResultModel<List<WatchList>>> UserWatchLists(string customerId)
        {

            return await _watchListsRepository.GetUserWatchLists(customerId);
        }
    }
}
