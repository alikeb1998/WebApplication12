using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using InstrumentDetail = Izi.Online.ViewModels.Instruments.InstrumentDetail;
using Instrument = Iz.Online.OmsModels.InputModels.Instruments.Instrument;

namespace Iz.Online.Services.Services
{
    public class InstrumentsService : IInstrumentsService
    {
        public IInstrumentsRepository _instrumentsRepository { get; set; }
        public IExternalInstrumentService _IExternalInstrumentsService { get; set; }


        public InstrumentsService(IInstrumentsRepository instrumentsRepository, IExternalInstrumentService externalInstrumentsService)
        {
            _instrumentsRepository = instrumentsRepository;
            _IExternalInstrumentsService = externalInstrumentsService;
        }

        public ResultModel<List<Instruments>> Instruments()
        {
            return _instrumentsRepository.GetInstrumentsList();
        }
        public ResultModel<List<InstrumentList>> InstrumentList()
        {
            var ins = _instrumentsRepository.GetInstrumentsList();

            if (!ins.IsSuccess)
                return new ResultModel<List<InstrumentList>>(null, false, "خطای پایگاه داده", -1);

            return new ResultModel<List<InstrumentList>>(ins.Model.Select(x => new InstrumentList()
            {
                Id = x.Id,
                Name = x.SymbolName.Substring(0, x.SymbolName.Length - 1),
                FullName = x.CompanyName,
                NscCode = x.Code,
                Bourse = x.Bourse,
                InstrumentId = x.InstrumentId,
                Tick = x.Tick
            }).ToList());

        }

        public ResultModel<List<WatchList>> UserWatchLists(string customerId)
        {

            return _instrumentsRepository.GetUserWatchLists(customerId);
        }

        public ResultModel<WatchListDetails> WatchListDetails(SearchWatchList model)
        {
            var wl  = _instrumentsRepository.GetWatchListDetails(model);
           

            foreach (var ins in wl.Model.Instruments)
            {
                
                var bestLimit = _IExternalInstrumentsService.BestLimits(new SelectedInstrument() { NscCode=ins.Code});
                var price = _IExternalInstrumentsService.Price(new SelectInstrumentDetails() { NscCode = ins.Code });

                ins.ClosePrice = price.Model.closingPrice;
                ins.AskPrice = bestLimit.Model.orderRow1.priceBestBuy;
                ins.BidPrice = bestLimit.Model.orderRow1.priceBestSale;
                var now = Convert.ToDouble(bestLimit.Model.orderRow1.priceBestBuy);
                var last = Convert.ToDouble(price.Model.lastPrice);
         
                ins.ChangePercent = (float)((now - last) / last) * 100;
                ins.LastPrice = price.Model.lastPrice;
                ins.SymbolName = ins.SymbolName.Substring(0, ins.SymbolName.Length - 1);
            }

            return wl;
        }


        public ResultModel<List<WatchList>> DeleteWatchList(SearchWatchList model)
        {
            return _instrumentsRepository.DeleteWatchList(model);
        }

        public ResultModel<WatchListDetails> NewWatchList(NewWatchList model)
        {
            var maxLen = Convert.ToInt32(_instrumentsRepository.GetAppConfigs("WatchListMaxLenName").Value);
            if (model.InstrumentsId.Count() > maxLen)
                return new ResultModel<WatchListDetails>(null, false, "حداکثر تعداد نماد در دیده بان" + maxLen + "است");

            if (model.WatchListName.Length > Convert.ToInt32(_instrumentsRepository.GetAppConfigs("WatchListMaxInstruments").Value))
                return new ResultModel<WatchListDetails>(null, false, "محدودیت در طول حروف نام دیده بان");
            
            if ( string.IsNullOrEmpty( model.CustomerId))
                return new ResultModel<WatchListDetails>(null, false, "مالک دیده بان مشخص نیست");

            if (string.IsNullOrEmpty(model.WatchListName))
                return new ResultModel<WatchListDetails>(null, false, "نام دیده بان اجباری است");

            if (model.InstrumentsId.Distinct().Count() != model.InstrumentsId.Count())
                return new ResultModel<WatchListDetails>(null, false, "وجود نماد تکراری در یک دیده بان");

            return _instrumentsRepository.NewWatchList(model);
        }

        public ResultModel<WatchListDetails> AddInstrumentToWatchList(EditEathListItems model)
        {
            var maxLen = Convert.ToInt32(_instrumentsRepository.GetAppConfigs("WatchListMaxInstruments").Value);

            var old = _instrumentsRepository.GetWatchListDetails(new SearchWatchList() { WatchListId = model.WatchListId });
            if (old.Model.Instruments.Count() >= maxLen)
                return new ResultModel<WatchListDetails>(null, false, "حداکثر تعداد نماد در دیده بان" + maxLen + "است");

            return _instrumentsRepository.AddInstrumentToWatchList(model);

        }

        public ResultModel<WatchListDetails> RemoveInstrumentFromWatchList(EditEathListItems model)
        {
            return _instrumentsRepository.RemoveInstrumentFromWatchList(model);

        }

        public ResultModel<List<WatchList>> InstrumentWatchLists(InstrumentWatchLists model)
        {
            
            return _instrumentsRepository.InstrumentWatchLists(model);

        }

        public ResultModel<InstrumentDetail> Detail(SelectInstrumentDetails model)
        {
            var result = new InstrumentDetail();
            

            var detail = _IExternalInstrumentsService.Details(model);
            var priceDetail = _IExternalInstrumentsService.Price(model);
            var insModel = new SelectedInstrument() { NscCode = model.NscCode };
            var bestLimit = _IExternalInstrumentsService.BestLimits(insModel);

            if (priceDetail.IsSuccess && priceDetail.Model != null)
            {
                result.closingPrice = priceDetail.Model.closingPrice;
                result.firstPrice = priceDetail.Model.firstPrice;
                result.lastPrice = priceDetail.Model.lastPrice;
                result.instrumentId = priceDetail.Model.instrumentId;
                result.lastTradeDate = priceDetail.Model.lastTradeDate;
                result.valueOfTrades = priceDetail.Model.valueOfTrades;
                result.numberOfTrades = Convert.ToInt32(priceDetail.Model.numberOfTrades);
                result.volumeOfTrades = Convert.ToInt32(priceDetail.Model.volumeOfTrades);
                result.yesterdayPrice = priceDetail.Model.yesterdayPrice;
                result.highPrice = priceDetail.Model.maximumPrice;
                result.lowPrice = priceDetail.Model.minimumPrice;

                result.ChangePercent = 10.2f;
                result.AskPrice = bestLimit.Model.orderRow1.priceBestBuy;
                result.BidPrice = bestLimit.Model.orderRow1.priceBestSale;
                
            }

            if (detail.IsSuccess && detail.Model != null)
            {
                result.State = detail.Model.State;
                result.GroupState = detail.Model.Group.State;
                result.PriceMax = detail.Model.PriceMax;
                result.PriceMin = detail.Model.PriceMin;
            }

            if (!(priceDetail.IsSuccess && detail.IsSuccess))
                return new ResultModel<InstrumentDetail>(null, false, priceDetail.Message, priceDetail.StatusCode);

            return new ResultModel<InstrumentDetail>(result);
        }

    }

}
