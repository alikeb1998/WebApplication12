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
        private readonly IInstrumentsRepository _instrumentsRepository;
        private readonly IExternalInstrumentService _externalInstrumentsService;


        public string _token { get; set; }

        public InstrumentsService(IInstrumentsRepository instrumentsRepository, IExternalInstrumentService externalInstrumentsService)
        {
            _instrumentsRepository = instrumentsRepository;
            _externalInstrumentsService = externalInstrumentsService;
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
                BuyCommissionRate = x.BuyCommisionRate,
                SellCommissionRate = x.SellCommisionRate,

            }).ToList()) ;

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
                result.PriceMin = detail.Model.PriceMax;


                result.Tick = detail.Model.Tick;
            }

            if (!(priceDetail.IsSuccess && detail.IsSuccess))
                return new ResultModel<InstrumentDetail>(null, false, priceDetail.Message, priceDetail.StatusCode);

            return new ResultModel<InstrumentDetail>(result);
        }

      
    }

}
