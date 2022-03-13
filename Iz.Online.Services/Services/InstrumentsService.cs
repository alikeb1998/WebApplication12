﻿using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.IExternalService;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using InstrumentDetail = Izi.Online.ViewModels.Instruments.InstrumentDetail;
using Instrument = Iz.Online.OmsModels.InputModels.Instruments.Instrument;
using DateHelper = Iz.Online.Files.DateHelper;
using Iz.Online.Files;
using Izi.Online.ViewModels.Instruments.BestLimit;

namespace Iz.Online.Services.Services
{
    public class InstrumentsService : IInstrumentsService
    {
        private readonly IInstrumentsRepository _instrumentsRepository;
        public IExternalInstrumentService _externalInstrumentsService { get; }
        public IExternalOrderService _externalOrderService { get; }

        private readonly ICacheService _cacheService;
        public InstrumentsService(IInstrumentsRepository instrumentsRepository, IExternalOrderService externalOrderService, IExternalInstrumentService externalInstrumentsService, ICacheService cacheService)
        {
            _instrumentsRepository = instrumentsRepository;
            _externalInstrumentsService = externalInstrumentsService;
            _cacheService = cacheService;
            _externalOrderService = externalOrderService;
        }


        public ResultModel<List<InstrumentList>> InstrumentList()
        {
            var result = _cacheService.InstrumentData().Select(x => new InstrumentList()
            {
                Bourse = x.Bourse,
                BuyCommissionRate = x.BuyCommissionRate,
                FullName = x.FullName,
                Name = x.Name,
                Id = x.Id,
                SellCommissionRate = x.SellCommissionRate,
                Tick = x.Tick
            }).ToList();
            return new ResultModel<List<InstrumentList>>(result);

        }


        public ResultModel<InstrumentDetail> Detail(int instrumentId)
        {
            var result = new InstrumentDetail();
            var instrumentDetails = _cacheService.InstrumentData(instrumentId);


            var detail = _externalInstrumentsService.Details(instrumentDetails.InstrumentId);
            if (!detail.IsSuccess || detail.Model == null)
                return new ResultModel<InstrumentDetail>(null, detail.StatusCode == 200, detail.Message, detail.StatusCode);

            var priceDetail = _externalInstrumentsService.Price(instrumentDetails.NscCode);
            if (!priceDetail.IsSuccess || priceDetail.Model == null)
                return new ResultModel<InstrumentDetail>(null, false, priceDetail.Message, priceDetail.StatusCode);

            var bestLimit = _externalInstrumentsService.BestLimits(instrumentDetails.NscCode, instrumentDetails.InstrumentId);
            if (!bestLimit.IsSuccess || bestLimit.Model == null)
                return new ResultModel<InstrumentDetail>(null, bestLimit.StatusCode == 200, bestLimit.Message, bestLimit.StatusCode);

            result.closingPrice = priceDetail.Model.closingPrice.Value;
            result.firstPrice = priceDetail.Model.firstPrice == null ? 0 : priceDetail.Model.firstPrice.Value;
            result.lastPrice = priceDetail.Model.lastPrice.Value;
            result.NscCode = priceDetail.Model.instrumentId;
            result.lastTradeDate = DateHelper.GetTimeFromString(priceDetail.Model.lastTradeDate);
            result.valueOfTrades = priceDetail.Model.valueOfTrades == null ? 0 : priceDetail.Model.valueOfTrades.Value;
            result.numberOfTrades = priceDetail.Model.numberOfTrades == null ? 0 : priceDetail.Model.numberOfTrades.Value;
            result.volumeOfTrades = priceDetail.Model.volumeOfTrades == null ? 0 : priceDetail.Model.volumeOfTrades.Value;
            result.yesterdayPrice = priceDetail.Model.yesterdayPrice == null ? 0 : priceDetail.Model.yesterdayPrice.Value;
            result.highPrice = priceDetail.Model.maximumPrice == null ? 0 : (long)priceDetail.Model.maximumPrice;
            result.lowPrice = priceDetail.Model.minimumPrice == null ? 0 : (long)priceDetail.Model.minimumPrice;


            var lastPrice = result.lastPrice;
            var yesterdayPrice = result.yesterdayPrice;

            if (yesterdayPrice > 0 && lastPrice > 0)
                result.LastPriceChangePercent = (float)((lastPrice - yesterdayPrice) / yesterdayPrice) * 100;

            result.AskPrice = bestLimit.Model.orderRow1.priceBestBuy;
            result.BidPrice = bestLimit.Model.orderRow1.priceBestSale;





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

        public ResultModel<bool> AddCommentToInstrument(AddCommentForInstrument model)
        {
            return _instrumentsRepository.AddCommentToInstrument(model);
        }

        public ResultModel<string> GetInstrumentComment(GetInstrumentComment model)
        {
            return _instrumentsRepository.GetInstrumentComment(model);
        }

        public void StartConsume()
        {
            _externalInstrumentsService.StartConsume();
        }

        public ResultModel<BestLimits> BestLimits(int InstrumentId, string hubId)
        {
            var instrumentDetails = _cacheService.InstrumentData(InstrumentId);

            _instrumentsRepository.CustomerSelectInstrument(new CustomerSelectInstrumentModel() { HubId = hubId, InstrumentId = InstrumentId });
            return _externalInstrumentsService.BestLimits(instrumentDetails.NscCode, instrumentDetails.InstrumentId);
        }

        public bool UpdateInstrumentsDb()
        {
            return _externalInstrumentsService.UpdateInstrumentList();

        }
    }

}
