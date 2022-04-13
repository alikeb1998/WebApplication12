
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.ResponsModels.Instruments;
using Iz.Online.Reopsitory.IRepository;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.Instruments.BestLimit;
using Izi.Online.ViewModels.ShareModels;
using BestLimits = Iz.Online.OmsModels.ResponsModels.BestLimits.BestLimits;
using Instruments = Iz.Online.OmsModels.ResponsModels.Instruments.Instruments;
using InstrumentDetails = Iz.Online.OmsModels.ResponsModels.Instruments.InstrumentDetails;
using InstrumentModel = Iz.Online.OmsModels.InputModels.Instruments.Instrument;
using InstrumentPrice = Iz.Online.OmsModels.ResponsModels.Instruments.InstrumentPrice;
using BestLimitsView = Izi.Online.ViewModels.Instruments.BestLimit.BestLimits;
using Izi.Online.ViewModels.SignalR;
using Iz.Online.OmsModels.ResponsModels.User;
using Microsoft.Extensions.Logging;


namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalInstrumentService : BaseService, IExternalInstrumentService
    {
        private readonly Microsoft.AspNetCore.SignalR.IHubContext<MainHub> _hubContext;

        private readonly IInstrumentsRepository _instrumentsRepository;
        private readonly IExternalOrderService _externalOrderService;
        private readonly IExternalUserService _externalUserService;
        private readonly ILogger<ExternalInstrumentService> _logger;
        //private readonly IPushService _pushService;
        private IHubUserService _hubUserService;


        public ExternalInstrumentService(IInstrumentsRepository instrumentsRepository, IExternalOrderService externalOrderService, IExternalUserService externalUserService
            , IHubUserService hubUserService, ILogger<ExternalInstrumentService> logger, Microsoft.AspNetCore.SignalR.IHubContext<MainHub> hubContext) : base(instrumentsRepository, ServiceProvider.Oms)
        {
            _instrumentsRepository = instrumentsRepository;
            _externalOrderService = externalOrderService;
            _hubUserService = hubUserService;
            _externalUserService = externalUserService;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<bool> UpdateInstrumentList()
        {
            var onDbInstrumentsList = (await _instrumentsRepository.GetInstrumentsList()).Model.Select(x => x.InstrumentId).ToList();
            var onDbInstrumentSector = (await _instrumentsRepository.GetInstrumentSector()).Model.ToList();
            var onDbInstrumentSubSectors = (await _instrumentsRepository.GetInstrumentSubSectors()).Model.ToList();
            var onDbInstrumentBourse = (await _instrumentsRepository.GetInstrumentBourse()).Model.ToList();


            var instruments = await HttpGetRequest<Instruments>("order/instruments");

            try
            {
                var receivedSector = instruments.instruments.Select(x => x.sector).DistinctBy(x => x.id).ToList();
                foreach (var sector in receivedSector)
                {
                    if (!onDbInstrumentSector.Select(x => x.SectorId).ToList().Contains(sector.id))
                    {
                        onDbInstrumentSector.Add(sector);
                        _instrumentsRepository.AddInstrumentSector(sector);
                    }
                }

                var receivedSubector = instruments.instruments.Select(x => x.subSector).DistinctBy(x => x.id).ToList();
                foreach (var subsector in receivedSubector)
                {
                    if (!onDbInstrumentSubSectors.Select(x => x.SubSectorId).ToList().Contains(subsector.id))
                    {
                        onDbInstrumentSubSectors.Add(subsector);
                        _instrumentsRepository.AddInstrumentSubSectors(subsector);
                    }
                }


                var receivedGroup = instruments.instruments.Select(x => x.group).DistinctBy(x => x.id).ToList();
                foreach (var group in receivedGroup)
                {

                    if (!onDbInstrumentBourse.Select(x => x.BourseId).ToList().Contains(group.id))
                    {
                        onDbInstrumentBourse.Add(group);
                        _instrumentsRepository.AddInstrumentBourse(group);
                    }
                }

                onDbInstrumentSector = (await _instrumentsRepository.GetInstrumentSector()).Model.ToList();
                onDbInstrumentSubSectors = (await _instrumentsRepository.GetInstrumentSubSectors()).Model.ToList();
                onDbInstrumentBourse = (await _instrumentsRepository.GetInstrumentBourse()).Model.ToList();



                foreach (var instrument in instruments.instruments)
                {
                    if (!onDbInstrumentsList.Contains(instrument.Id))
                    {
                        onDbInstrumentsList.Add(instrument.Id);
                        await _instrumentsRepository.AddInstrument(instrument
                             , onDbInstrumentSector.FirstOrDefault(x => x.SectorId == instrument.sector.id).Id
                             , onDbInstrumentSubSectors.FirstOrDefault(x => x.SubSectorId == instrument.subSector.id).Id
                             , onDbInstrumentBourse.FirstOrDefault(x => x.BourseId == instrument.group.id).Id
                             , instrument.tick
                             , 0.003712f
                             , 0.0038f
                             );

                    }
                    else
                    {

                        await _instrumentsRepository.UpdateInstruments(instrument
                             , onDbInstrumentSector.FirstOrDefault(x => x.SectorId == instrument.sector.id).Id
                             , onDbInstrumentSubSectors.FirstOrDefault(x => x.SubSectorId == instrument.subSector.id).Id
                             , onDbInstrumentBourse.FirstOrDefault(x => x.BourseId == instrument.group.id).Id
                             , instrument.tick
                             , 0.003712f
                             , 0.0038f
                             );
                    }

                }

                return true;
            }
            catch (Exception e)
            {
                _instrumentsRepository.LogException(e);
                return false;
            }

        }
        public async Task StartConsume()
        {
            await _hubUserService.CreateAllConsumers();
        }

        public async Task<ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits>> BestLimits(string NscCode, long InstrumentId, string hubId)
        {



            _logger.LogError("before get nationalCode");
            var nationalCode =   HttpGetRequest<CustomerInfo>("Customer/GetCustomerInfo");
            _logger.LogError("after get nationalCode");
            MainHub.NatCode = nationalCode.Result.nationalID;
            //if (nationalCode != null)
            //{
            //     Task.Run(() => _hubUserService.ConsumeRefreshInstrumentBestLimit_Orginal(NscCode, nationalCode.Result.nationalID));
            //}
            _logger.LogError("before get bestlimit");
            var bestLimit = await HttpGetRequest<BestLimits>($"rlc/best-limit/{NscCode}");
            _logger.LogError("after get bestlimit");
            if (bestLimit.bestLimit == null || bestLimit.statusCode != 200)
                return new ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits>(null, bestLimit.statusCode == 200, bestLimit.clientMessage, bestLimit.statusCode);
           
            _logger.LogError("before get detail");
            var detail = await Details(InstrumentId, NscCode, hubId);
            _logger.LogError("after get detail");
            if (detail.Model == null || detail.StatusCode != 200)
                return new ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits>(null, detail.StatusCode == 200, detail.Message, detail.StatusCode);
            

            _logger.LogError("before get actives");
            var activeOrders = await _externalOrderService.GetAllActives();
            _logger.LogError("after get actives");
            if (activeOrders.Model.Orders == null || activeOrders.StatusCode != 200)
                return new ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits>(null, bestLimit.statusCode == 200, bestLimit.clientMessage, bestLimit.statusCode);
            _logger.LogError("before proccess bestilimit");
            var result = new BestLimitsView()
            {
                orderRow1 = new OrderRow()
                {
                    countBestBuy = bestLimit.bestLimit.orderRow1 == null ? 0 : bestLimit.bestLimit.orderRow1.countBestBuy,
                    countBestSale = bestLimit.bestLimit.orderRow1 == null ? 0 : bestLimit.bestLimit.orderRow1.countBestSale,
                    priceBestBuy = bestLimit.bestLimit.orderRow1 == null ? 0 : bestLimit.bestLimit.orderRow1.priceBestBuy,
                    priceBestSale = bestLimit.bestLimit.orderRow1 == null ? 0 : bestLimit.bestLimit.orderRow1.priceBestSale,
                    volumeBestBuy = bestLimit.bestLimit.orderRow1 == null ? 0 : bestLimit.bestLimit.orderRow1.volumeBestBuy,
                    volumeBestSale = bestLimit.bestLimit.orderRow1 == null ? 0 : bestLimit.bestLimit.orderRow1.volumeBestSale
                },
                orderRow2 = new OrderRow()
                {
                    countBestBuy = bestLimit.bestLimit.orderRow2 == null ? 0 : bestLimit.bestLimit.orderRow2.countBestBuy,
                    countBestSale = bestLimit.bestLimit.orderRow2 == null ? 0 : bestLimit.bestLimit.orderRow2.countBestSale,
                    priceBestBuy = bestLimit.bestLimit.orderRow2 == null ? 0 : bestLimit.bestLimit.orderRow2.priceBestBuy,
                    priceBestSale = bestLimit.bestLimit.orderRow2 == null ? 0 : bestLimit.bestLimit.orderRow2.priceBestSale,
                    volumeBestBuy = bestLimit.bestLimit.orderRow2 == null ? 0 : bestLimit.bestLimit.orderRow2.volumeBestBuy,
                    volumeBestSale = bestLimit.bestLimit.orderRow2 == null ? 0 : bestLimit.bestLimit.orderRow2.volumeBestSale
                },
                orderRow3 = new OrderRow()
                {
                    countBestBuy = bestLimit.bestLimit.orderRow3 == null ? 0 : bestLimit.bestLimit.orderRow3.countBestBuy,
                    countBestSale = bestLimit.bestLimit.orderRow3 == null ? 0 : bestLimit.bestLimit.orderRow3.countBestSale,
                    priceBestBuy = bestLimit.bestLimit.orderRow3 == null ? 0 : bestLimit.bestLimit.orderRow3.priceBestBuy,
                    priceBestSale = bestLimit.bestLimit.orderRow3 == null ? 0 : bestLimit.bestLimit.orderRow3.priceBestSale,
                    volumeBestBuy = bestLimit.bestLimit.orderRow3 == null ? 0 : bestLimit.bestLimit.orderRow3.volumeBestBuy,
                    volumeBestSale = bestLimit.bestLimit.orderRow3 == null ? 0 : bestLimit.bestLimit.orderRow3.volumeBestSale
                },
                orderRow4 = new OrderRow()
                {
                    countBestBuy = bestLimit.bestLimit.orderRow4 == null ? 0 : bestLimit.bestLimit.orderRow4.countBestBuy,
                    countBestSale = bestLimit.bestLimit.orderRow4 == null ? 0 : bestLimit.bestLimit.orderRow4.countBestSale,
                    priceBestBuy = bestLimit.bestLimit.orderRow4 == null ? 0 : bestLimit.bestLimit.orderRow4.priceBestBuy,
                    priceBestSale = bestLimit.bestLimit.orderRow4 == null ? 0 : bestLimit.bestLimit.orderRow4.priceBestSale,
                    volumeBestBuy = bestLimit.bestLimit.orderRow4 == null ? 0 : bestLimit.bestLimit.orderRow4.volumeBestBuy,
                    volumeBestSale = bestLimit.bestLimit.orderRow4 == null ? 0 : bestLimit.bestLimit.orderRow4.volumeBestSale
                },
                orderRow5 = new OrderRow()
                {
                    countBestBuy = bestLimit.bestLimit.orderRow5 == null ? 0 : bestLimit.bestLimit.orderRow5.countBestBuy,
                    countBestSale = bestLimit.bestLimit.orderRow5 == null ? 0 : bestLimit.bestLimit.orderRow5.countBestSale,
                    priceBestBuy = bestLimit.bestLimit.orderRow5 == null ? 0 : bestLimit.bestLimit.orderRow5.priceBestBuy,
                    priceBestSale = bestLimit.bestLimit.orderRow5 == null ? 0 : bestLimit.bestLimit.orderRow5.priceBestSale,
                    volumeBestBuy = bestLimit.bestLimit.orderRow5 == null ? 0 : bestLimit.bestLimit.orderRow5.volumeBestBuy,
                    volumeBestSale = bestLimit.bestLimit.orderRow5 == null ? 0 : bestLimit.bestLimit.orderRow5.volumeBestSale
                },
                orderRow6 = new OrderRow()
                {
                    countBestBuy = bestLimit.bestLimit.orderRow6 == null ? 0 : bestLimit.bestLimit.orderRow6.countBestBuy,
                    countBestSale = bestLimit.bestLimit.orderRow6 == null ? 0 : bestLimit.bestLimit.orderRow6.countBestSale,
                    priceBestBuy = bestLimit.bestLimit.orderRow6 == null ? 0 : bestLimit.bestLimit.orderRow6.priceBestBuy,
                    priceBestSale = bestLimit.bestLimit.orderRow6 == null ? 0 : bestLimit.bestLimit.orderRow6.priceBestSale,
                    volumeBestBuy = bestLimit.bestLimit.orderRow6 == null ? 0 : bestLimit.bestLimit.orderRow6.volumeBestBuy,
                    volumeBestSale = bestLimit.bestLimit.orderRow6 == null ? 0 : bestLimit.bestLimit.orderRow6.volumeBestSale
                },

            };
            _logger.LogError("after proccess bestilimit");
            _logger.LogError("before proccess acives");
            foreach (var order in activeOrders.Model.Orders)
            {

                if (NscCode == order.instrument.code)
                {
                    result.orderRow1.HasOrderBuy = !result.orderRow1.HasOrderBuy ? order.price == result.orderRow1.priceBestBuy : true;
                    result.orderRow2.HasOrderBuy = !result.orderRow2.HasOrderBuy ? order.price == result.orderRow2.priceBestBuy : true;
                    result.orderRow3.HasOrderBuy = !result.orderRow3.HasOrderBuy ? order.price == result.orderRow3.priceBestBuy : true;
                    result.orderRow4.HasOrderBuy = !result.orderRow4.HasOrderBuy ? order.price == result.orderRow4.priceBestBuy : true;
                    result.orderRow5.HasOrderBuy = !result.orderRow5.HasOrderBuy ? order.price == result.orderRow5.priceBestBuy : true;
                    result.orderRow6.HasOrderBuy = !result.orderRow6.HasOrderBuy ? order.price == result.orderRow6.priceBestBuy : true;

                    result.orderRow1.HasOrderSell = !result.orderRow1.HasOrderSell ? order.price == result.orderRow1.priceBestSale : true;
                    result.orderRow2.HasOrderSell = !result.orderRow2.HasOrderSell ? order.price == result.orderRow2.priceBestSale : true;
                    result.orderRow3.HasOrderSell = !result.orderRow3.HasOrderSell ? order.price == result.orderRow3.priceBestSale : true;
                    result.orderRow4.HasOrderSell = !result.orderRow4.HasOrderSell ? order.price == result.orderRow4.priceBestSale : true;
                    result.orderRow5.HasOrderSell = !result.orderRow5.HasOrderSell ? order.price == result.orderRow5.priceBestSale : true;
                    result.orderRow6.HasOrderSell = !result.orderRow6.HasOrderSell ? order.price == result.orderRow6.priceBestSale : true;
                }
            }
            _logger.LogError("after proccess actives");
            _logger.LogError("before proccess ");
            var proccessedResult = ProccessVolume(result, detail.Model);
            _logger.LogError("after proccess");
            return new ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits>(proccessedResult);
        }

        public async Task<ResultModel<InstrumentPriceDetails>> Price(string NscCode)
        {
             Task.Run(() => _hubUserService.PushPrice_Original(NscCode));
            var result = await HttpGetRequest<InstrumentPrice>($"rlc/price/{NscCode}");


            return new ResultModel<InstrumentPriceDetails>(result.price, result.statusCode == 200, result.clientMessage, result.statusCode);
        }

        public async Task<ResultModel<Details>> Details(long InstrumentId, string nscCode, string hubId)
        {
            _logger.LogError("before get detailOrder");
            var result = await HttpGetRequest<InstrumentDetails>($"order/instrument/{InstrumentId}");
            _logger.LogError("after get detailOrder");
           // var nationalCode = HttpGetRequest<CustomerInfo>("Customer/GetCustomerInfo");
            return new ResultModel<Details>(result.Instrument, result.statusCode == 200, result.clientMessage, result.statusCode);

        }
        private BestLimitsView ProccessVolume(BestLimitsView bestLimits, Details detail)
        {
            var totalBuys = bestLimits.orderRow1.volumeBestBuy +
                            bestLimits.orderRow2.volumeBestBuy +
                            bestLimits.orderRow3.volumeBestBuy +
                            bestLimits.orderRow4.volumeBestBuy +
                            bestLimits.orderRow5.volumeBestBuy +
                            bestLimits.orderRow6.volumeBestBuy;

            var totalSells = bestLimits.orderRow1.volumeBestSale +
                      bestLimits.orderRow2.volumeBestSale +
                      bestLimits.orderRow3.volumeBestSale +
                      bestLimits.orderRow4.volumeBestSale +
                      bestLimits.orderRow5.volumeBestSale +
                      bestLimits.orderRow6.volumeBestSale;

            bestLimits.orderRow1.BuyWeight = bestLimits.orderRow1.priceBestBuy != 0 ? PercentProccessor(totalBuys, bestLimits.orderRow1.volumeBestBuy) : 0;
            bestLimits.orderRow2.BuyWeight = bestLimits.orderRow2.priceBestBuy != 0 ? PercentProccessor(totalBuys, bestLimits.orderRow2.volumeBestBuy) : 0;
            bestLimits.orderRow3.BuyWeight = bestLimits.orderRow3.priceBestBuy != 0 ? PercentProccessor(totalBuys, bestLimits.orderRow3.volumeBestBuy) : 0;
            bestLimits.orderRow4.BuyWeight = bestLimits.orderRow4.priceBestBuy != 0 ? PercentProccessor(totalBuys, bestLimits.orderRow4.volumeBestBuy) : 0;
            bestLimits.orderRow5.BuyWeight = bestLimits.orderRow5.priceBestBuy != 0 ? PercentProccessor(totalBuys, bestLimits.orderRow5.volumeBestBuy) : 0;
            bestLimits.orderRow6.BuyWeight = bestLimits.orderRow6.priceBestBuy != 0 ? PercentProccessor(totalBuys, bestLimits.orderRow6.volumeBestBuy) : 0;

            bestLimits.orderRow1.SellWeight = bestLimits.orderRow1.priceBestSale != 0 ? PercentProccessor(totalSells, bestLimits.orderRow1.volumeBestSale) : 0;
            bestLimits.orderRow2.SellWeight = bestLimits.orderRow2.priceBestSale != 0 ? PercentProccessor(totalSells, bestLimits.orderRow2.volumeBestSale) : 0;
            bestLimits.orderRow3.SellWeight = bestLimits.orderRow3.priceBestSale != 0 ? PercentProccessor(totalSells, bestLimits.orderRow3.volumeBestSale) : 0;
            bestLimits.orderRow4.SellWeight = bestLimits.orderRow4.priceBestSale != 0 ? PercentProccessor(totalSells, bestLimits.orderRow4.volumeBestSale) : 0;
            bestLimits.orderRow5.SellWeight = bestLimits.orderRow5.priceBestSale != 0 ? PercentProccessor(totalSells, bestLimits.orderRow5.volumeBestSale) : 0;
            bestLimits.orderRow6.SellWeight = bestLimits.orderRow6.priceBestSale != 0 ? PercentProccessor(totalSells, bestLimits.orderRow6.volumeBestSale) : 0;

            if (bestLimits.orderRow1.priceBestBuy == detail.PriceMax)
            {
                bestLimits.IsBuyQueue = true;
                bestLimits.IsSellQueue = false;
            }
            else if (bestLimits.orderRow1.priceBestSale == detail.PriceMin)
            {
                bestLimits.IsSellQueue = true;
                bestLimits.IsBuyQueue = false;
            }

            return bestLimits;
        }
        private double PercentProccessor(double a, double b)
        {
            if (a == 0) return 0;
            var res = (a - b) / a * 100;
            return 100 - res;
        }


    }
}
