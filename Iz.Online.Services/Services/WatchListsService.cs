using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.IExternalService;
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

      public  IExternalInstrumentService _externalInstrumentsServices  {get;}
      public  IExternalOrderService _externalOrderService {get;}

        public WatchListsService(IWatchListsRepository watchListsRepository,IExternalInstrumentService externalInstrumentsService, IExternalOrderService externalOrderService , ICacheService cacheService)
        {
            _watchListsRepository = watchListsRepository;
            _externalInstrumentsService = externalInstrumentsService;
            _cache = cacheService;
            _externalInstrumentsServices = externalInstrumentsService;
            _externalOrderService = externalOrderService;
        }

        public ResultModel<WatchListDetails> WatchListDetails(SearchWatchList model)
        {
            var wl = _watchListsRepository.GetWatchListDetails(model);


            foreach (var ins in wl.Model.Instruments)
            {
               var instrumentDetails =  _cache.InstrumentData((int) ins.Id);
                var bestLimit = _externalInstrumentsService.BestLimits(instrumentDetails.NscCode, instrumentDetails.InstrumentId);
                var price = _externalInstrumentsService.Price(instrumentDetails.NscCode);

                if (!(bestLimit.IsSuccess && price.IsSuccess))
                    continue;

                if (bestLimit.Model == null || price.Model == null)
                    continue;

                ins.ClosePrice = price.Model.closingPrice.Value;
                ins.AskPrice = bestLimit.Model.orderRow1.priceBestBuy;
                ins.BidPrice = bestLimit.Model.orderRow1.priceBestSale;
                ins.BuyVolume = bestLimit.Model.orderRow1.volumeBestBuy;
                ins.SellVolume = bestLimit.Model.orderRow1.volumeBestSale;
                var lastPrice = price.Model.lastPrice.Value;
                var yesterdayPrice = price.Model.yesterdayPrice;

                if (yesterdayPrice > 0 && lastPrice > 0)
                    ins.LastPriceChangePercent = (float)((lastPrice - yesterdayPrice) / yesterdayPrice) * 100;

                ins.LastPrice = price.Model.lastPrice.Value;
                var name = ins.SymbolName;
                ins.SymbolName = name.EndsWith("1") ? name.Substring(0, ins.SymbolName.Length - 1) : name;
                ins.InstrumentId = instrumentDetails.InstrumentId;
            }

            return wl;
        }


        public ResultModel<List<WatchList>> DeleteWatchList(SearchWatchList model)
        {
            return _watchListsRepository.DeleteWatchList(model);
        }

        public ResultModel<WatchListDetails> NewWatchList(NewWatchList model)
        {
            model.WatchListName = model.WatchListName.Trim();

            if (ValidateWatchList(model, out var resultModel))
                return resultModel;

            return _watchListsRepository.NewWatchList(model);
        }

        private bool ValidateWatchList(NewWatchList model, out ResultModel<WatchListDetails> resultModel)
        {
            var maxLen = Convert.ToInt32(_cache.ConfigData("WatchListMaxLenName").Value);

            var oldWl = _watchListsRepository.GetUserWatchLists(model.CustomerId);
            if (oldWl.IsSuccess)
                if (oldWl.Model.Select(x => x.WatchListName).Contains(model.WatchListName))
                {
                    resultModel = new ResultModel<WatchListDetails>(null, false, "نام دیده بان تکراری است");
                    return true;
                }

            if (model.Id.Count() > maxLen)
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "حداکثر تعداد نماد در دیده بان" + maxLen + "است");
                return true;
            }

            if (model.WatchListName.Length >
                Convert.ToInt32(_cache.ConfigData("WatchListMaxInstruments").Value))
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "محدودیت در طول حروف نام دیده بان");
                return true;
            }

            if (string.IsNullOrEmpty(model.CustomerId))
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "مالک دیده بان مشخص نیست");
                return true;
            }

            if (string.IsNullOrEmpty(model.WatchListName))
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "نام دیده بان اجباری است");
                return true;
            }

            if (model.Id.Distinct().Count() != model.Id.Count())
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "وجود نماد تکراری در یک دیده بان");
                return true;
            }

            resultModel = new ResultModel<WatchListDetails>(null);
            return false;
        }
        private bool ValidateWatchList(EditWatchList model, out ResultModel<WatchListDetails> resultModel)
        {
            var maxLen = Convert.ToInt32(_cache.ConfigData("WatchListMaxLenName").Value);

            var oldWl = _watchListsRepository.GetUserWatchLists(model.CustomerId);
            if (oldWl.IsSuccess)
                if (oldWl.Model.Where(x => x.Id != model.WatchListId).Select(x => x.WatchListName).Contains(model.WatchListName))
                {
                    resultModel = new ResultModel<WatchListDetails>(null, false, "نام دیده بان تکراری است");
                    return true;
                }

            var entity = _watchListsRepository.GetWatchListDetails(new SearchWatchList()
            {
                CustomerId = model.CustomerId,
                WatchListId = model.WatchListId

            });

            if (!entity.IsSuccess)
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "دیده بان یافت نشد");
                return true;
            }
            if (model.Id.Count() > maxLen)
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "حداکثر تعداد نماد در دیده بان" + maxLen + "است");
                return true;
            }

            if (model.WatchListName.Length >
                Convert.ToInt32(_cache.ConfigData("WatchListMaxInstruments").Value))
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "محدودیت در طول حروف نام دیده بان");
                return true;
            }

            if (string.IsNullOrEmpty(model.CustomerId))
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "مالک دیده بان مشخص نیست");
                return true;
            }

            if (string.IsNullOrEmpty(model.WatchListName))
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "نام دیده بان اجباری است");
                return true;
            }

            if (model.Id.Distinct().Count() != model.Id.Count())
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "وجود نماد تکراری در یک دیده بان");
                return true;
            }

            resultModel = new ResultModel<WatchListDetails>(null);
            return false;
        }

        public ResultModel<WatchListDetails> AddInstrumentToWatchList(EditEathListItems model)
        {
            var maxLen = Convert.ToInt32(_cache.ConfigData("WatchListMaxInstruments").Value);

            var old = _watchListsRepository.GetWatchListDetails(new SearchWatchList() { WatchListId = model.WatchListId });
            if (old.Model.Instruments.Count() >= maxLen)
                return new ResultModel<WatchListDetails>(null, false, "حداکثر تعداد نماد در دیده بان" + maxLen + "است");

            return _watchListsRepository.AddInstrumentToWatchList(model);

        }

        public ResultModel<WatchListDetails> RemoveInstrumentFromWatchList(EditEathListItems model)
        {
            return _watchListsRepository.RemoveInstrumentFromWatchList(model);

        }

        public ResultModel<List<WatchList>> InstrumentWatchLists(InstrumentWatchLists model)
        {

            return _watchListsRepository.InstrumentWatchLists(model);

        }
        public ResultModel<WatchListDetails> UpdateWatchList(EditWatchList model)
        {
            model.WatchListName = model.WatchListName.Trim();

            if (ValidateWatchList(model, out var resultModel))
                return resultModel;

            return _watchListsRepository.UpdateWatchList(model);

        }

        

        public ResultModel<List<WatchList>> UserWatchLists(string customerId)
        {
            return _watchListsRepository.GetUserWatchLists(customerId);
        }
    }
}
