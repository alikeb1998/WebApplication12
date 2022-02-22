using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using InstrumentDetail = Izi.Online.ViewModels.Instruments.InstrumentDetail;
using Instrument = Iz.Online.OmsModels.InputModels.Instruments.Instrument;
using DateHelper = Iz.Online.Files.DateHelper;
using Iz.Online.Files;

namespace Iz.Online.Services.Services
{
    public class InstrumentsService : IInstrumentsService
    {
        public IInstrumentsRepository _instrumentsRepository { get; set; }
        public IExternalInstrumentService _externalInstrumentsService { get; set; }
        public string _token { get; set; }


        public InstrumentsService(IInstrumentsRepository instrumentsRepository, IExternalInstrumentService externalInstrumentsService)
        {
            _instrumentsRepository = instrumentsRepository;
            _externalInstrumentsService = externalInstrumentsService;
            _externalInstrumentsService._token = _token;
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
                Name = x.SymbolName.EndsWith("1") ? x.SymbolName.Substring(0, x.SymbolName.Length - 1) : x.SymbolName,
                FullName = x.CompanyName,
                NscCode = x.Code,
                Bourse = x.Bourse,
                InstrumentId = x.InstrumentId,
                Tick = x.Tick,

            }).ToList());

        }

        public ResultModel<List<WatchList>> UserWatchLists(string customerId)
        {

            return _instrumentsRepository.GetUserWatchLists(customerId);
        }

        public ResultModel<WatchListDetails> WatchListDetails(SearchWatchList model)
        {
            var wl = _instrumentsRepository.GetWatchListDetails(model);


            foreach (var ins in wl.Model.Instruments)
            {

                var bestLimit = _externalInstrumentsService.BestLimits(new SelectedInstrument() { NscCode = ins.Code });
                var price = _externalInstrumentsService.Price(new SelectInstrumentDetails() { NscCode = ins.Code });

                if (!(bestLimit.IsSuccess && price.IsSuccess))
                    continue;

                if (bestLimit.Model == null || price.Model == null)
                    continue;

                ins.ClosePrice = price.Model.closingPrice.Value;
                ins.AskPrice = bestLimit.Model.orderRow1.priceBestSale;
                ins.BidPrice = bestLimit.Model.orderRow1.priceBestBuy;
                var lastPrice = price.Model.lastPrice.Value;
                var yesterdayPrice = price.Model.yesterdayPrice;

                if (yesterdayPrice > 0 && lastPrice > 0)
                    ins.LastPriceChangePercent = (float)((lastPrice - yesterdayPrice) / yesterdayPrice) * 100;

                ins.LastPrice = price.Model.lastPrice.Value;
                var name = ins.SymbolName;
                ins.SymbolName = name.EndsWith("1") ? name.Substring(0, ins.SymbolName.Length - 1) : name;
            }

            return wl;
        }


        public ResultModel<List<WatchList>> DeleteWatchList(SearchWatchList model)
        {
            return _instrumentsRepository.DeleteWatchList(model);
        }

        public ResultModel<WatchListDetails> NewWatchList(NewWatchList model)
        {
            model.WatchListName = model.WatchListName.Trim();

            if (ValidateWatchList(model, out var resultModel))
                return resultModel;

            return _instrumentsRepository.NewWatchList(model);
        }

        private bool ValidateWatchList(NewWatchList model, out ResultModel<WatchListDetails> resultModel)
        {
            var maxLen = Convert.ToInt32(_instrumentsRepository.GetAppConfigs("WatchListMaxLenName").Value);

            var oldWl = _instrumentsRepository.GetUserWatchLists(model.CustomerId);
            if (oldWl.IsSuccess)
                if (oldWl.Model.Select(x => x.WatchListName).Contains(model.WatchListName))
                {
                    resultModel = new ResultModel<WatchListDetails>(null, false, "نام دیده بان تکراری است");
                    return true;
                }

            if (model.InstrumentsId.Count() > maxLen)
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "حداکثر تعداد نماد در دیده بان" + maxLen + "است");
                return true;
            }

            if (model.WatchListName.Length >
                Convert.ToInt32(_instrumentsRepository.GetAppConfigs("WatchListMaxInstruments").Value))
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

            if (model.InstrumentsId.Distinct().Count() != model.InstrumentsId.Count())
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "وجود نماد تکراری در یک دیده بان");
                return true;
            }

            resultModel = new ResultModel<WatchListDetails>(null);
            return false;
        }
        private bool ValidateWatchList(EditWatchList model, out ResultModel<WatchListDetails> resultModel)
        {
            var maxLen = Convert.ToInt32(_instrumentsRepository.GetAppConfigs("WatchListMaxLenName").Value);

            var oldWl = _instrumentsRepository.GetUserWatchLists(model.CustomerId);
            if (oldWl.IsSuccess)
                if (oldWl.Model.Where(x => x.Id != model.Id).Select(x => x.WatchListName).Contains(model.WatchListName))
                {
                    resultModel = new ResultModel<WatchListDetails>(null, false, "نام دیده بان تکراری است");
                    return true;
                }

            var entity = _instrumentsRepository.GetWatchListDetails(new SearchWatchList()
            {
                CustomerId = model.CustomerId,
                WatchListId = model.Id

            });

            if (!entity.IsSuccess)
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "دیده بان یافت نشد");
                return true;
            }
            if (model.InstrumentsId.Count() > maxLen)
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "حداکثر تعداد نماد در دیده بان" + maxLen + "است");
                return true;
            }

            if (model.WatchListName.Length >
                Convert.ToInt32(_instrumentsRepository.GetAppConfigs("WatchListMaxInstruments").Value))
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

            if (model.InstrumentsId.Distinct().Count() != model.InstrumentsId.Count())
            {
                resultModel = new ResultModel<WatchListDetails>(null, false, "وجود نماد تکراری در یک دیده بان");
                return true;
            }

            resultModel = new ResultModel<WatchListDetails>(null);
            return false;
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


            var detail = _externalInstrumentsService.Details(model);
            var priceDetail = _externalInstrumentsService.Price(model);

            var bestLimit = _externalInstrumentsService.BestLimits(new SelectedInstrument() { NscCode = model.NscCode });

            if (priceDetail.IsSuccess && priceDetail.Model != null)
            {
                result.closingPrice = priceDetail.Model.closingPrice.Value;
                result.firstPrice =  priceDetail.Model.firstPrice == null ? 0 : priceDetail.Model.firstPrice.Value;
                result.lastPrice = priceDetail.Model.lastPrice.Value;
                result.NscCode = priceDetail.Model.instrumentId;
                result.lastTradeDate = DateHelper.GetTimeFromString(priceDetail.Model.lastTradeDate);
                result.valueOfTrades = priceDetail.Model.valueOfTrades.Value;
                result.numberOfTrades = priceDetail.Model.numberOfTrades.Value;
                result.volumeOfTrades = priceDetail.Model.volumeOfTrades.Value;
                result.yesterdayPrice = priceDetail.Model.yesterdayPrice.Value;
                result.highPrice = (long)priceDetail.Model.maximumPrice;
                result.lowPrice = (long)priceDetail.Model.minimumPrice;

                var lastPrice = priceDetail.Model.lastPrice.Value;
                var yesterdayPrice = priceDetail.Model.yesterdayPrice;

                if (yesterdayPrice > 0 && lastPrice > 0)
                    result.LastPriceChangePercent = (float)((lastPrice - yesterdayPrice) / yesterdayPrice) * 100;
                
                result.AskPrice = bestLimit.Model.orderRow1.priceBestBuy;
                result.BidPrice = bestLimit.Model.orderRow1.priceBestSale;
                
                

            }

            if (detail.IsSuccess && detail.Model != null)
            {
                result.State = detail.Model.State;
                result.StateText = EnumHelper.InstrumentStates(result.State.ToString());
                result.GroupState = detail.Model.Group.State;
                result.GroupStateText = EnumHelper.InstrumentGroupStates(result.GroupState.ToString());
                result.PriceMax = detail.Model.PriceMax;
                result.PriceMin = detail.Model.PriceMin;


                result.Tick = detail.Model.Tick;
            }

            if (!(priceDetail.IsSuccess && detail.IsSuccess))
                return new ResultModel<InstrumentDetail>(null, false, priceDetail.Message, priceDetail.StatusCode);

            return new ResultModel<InstrumentDetail>(result);
        }

        public ResultModel<WatchListDetails> UpdateWatchList(EditWatchList model)
        {
            model.WatchListName = model.WatchListName.Trim();

            if (ValidateWatchList(model, out var resultModel))
                return resultModel;

            return _instrumentsRepository.UpdateWatchList(model);

        }

        public ResultModel<bool> AddCommentToInstrument(AddCommentForInstrument model)
        {
            return _instrumentsRepository.AddCommentToInstrument(model);
        }

        public ResultModel<string> GetInstrumentComment(GetInstrumentComment model)
        {
            return _instrumentsRepository.GetInstrumentComment(model);
        }
    }

}
