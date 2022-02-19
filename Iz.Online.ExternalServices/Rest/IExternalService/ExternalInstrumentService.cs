using Iz.Online.ExternalServices.Push.IKafkaPushServices;
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

namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalInstrumentService : BaseService, IExternalInstrumentService
    {
        private readonly IInstrumentsRepository _instrumentsRepository;
        private IExternalOrderService _externalOrderService;
        private readonly IPushService _pushService;
        public ExternalInstrumentService(IInstrumentsRepository instrumentsRepository, IPushService pushService, IExternalOrderService externalOrderService) : base(instrumentsRepository)
        {
            _instrumentsRepository = instrumentsRepository;
            _externalOrderService = externalOrderService;
            _pushService = pushService;
        }

        public bool UpdateInstrumentList()
        {
            var onDbInstrumentsList = _instrumentsRepository.GetInstrumentsList().Model.Select(x => x.InstrumentId).ToList();
            var onDbInstrumentSector = _instrumentsRepository.GetInstrumentSector().Model.ToList();
            var onDbInstrumentSubSectors = _instrumentsRepository.GetInstrumentSubSectors().Model.ToList();
            var onDbInstrumentBourse = _instrumentsRepository.GetInstrumentBourse().Model.ToList();


            var instruments = HttpGetRequest<Instruments>("order/instruments");

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

                onDbInstrumentSector = _instrumentsRepository.GetInstrumentSector().Model.ToList();
                onDbInstrumentSubSectors = _instrumentsRepository.GetInstrumentSubSectors().Model.ToList();
                onDbInstrumentBourse = _instrumentsRepository.GetInstrumentBourse().Model.ToList();



                foreach (var instrument in instruments.instruments)
                {
                    if (!onDbInstrumentsList.Contains(instrument.Id))
                    {
                        onDbInstrumentsList.Add(instrument.Id);
                        _instrumentsRepository.AddInstrument(instrument
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
                        
                        _instrumentsRepository.UpdateInstruments(instrument
                            , onDbInstrumentSector.FirstOrDefault(x => x.SectorId == instrument.sector.id).Id
                            , onDbInstrumentSubSectors.FirstOrDefault(x => x.SubSectorId == instrument.subSector.id).Id
                            , onDbInstrumentBourse.FirstOrDefault(x => x.BourseId == instrument.group.id).Id
                            , instrument.tick
                            , 0.003712f
                            ,0.0038f
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

        public ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits> BestLimits(SelectedInstrument model)
        {
            //model.InstrumentId = "IRO1FOLD0001";

            Task.Run(async () => _pushService.ConsumeRefreshInstrumentBestLimit(model.NscCode));

            var bestLimit = HttpGetRequest<BestLimits>($"rlc/best-limit/{model.NscCode}");
           
            if (bestLimit.bestLimit == null || bestLimit.statusCode != 200)
                return new ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits>(null, false, bestLimit.clientMessage, bestLimit.statusCode);

            var result = new BestLimitsView()
            {
                orderRow1 = new OrderRow()
                {
                    countBestBuy = bestLimit.bestLimit.orderRow1.countBestBuy,
                    countBestSale = bestLimit.bestLimit.orderRow1.countBestSale,
                    priceBestBuy = bestLimit.bestLimit.orderRow1.priceBestBuy,
                    priceBestSale = bestLimit.bestLimit.orderRow1.priceBestSale,
                    volumeBestBuy = bestLimit.bestLimit.orderRow1.volumeBestBuy,
                    volumeBestSale = bestLimit.bestLimit.orderRow1.volumeBestSale
                },
                orderRow2 = new OrderRow()
                {
                    countBestBuy = bestLimit.bestLimit.orderRow2.countBestBuy,
                    countBestSale = bestLimit.bestLimit.orderRow2.countBestSale,
                    priceBestBuy = bestLimit.bestLimit.orderRow2.priceBestBuy,
                    priceBestSale = bestLimit.bestLimit.orderRow2.priceBestSale,
                    volumeBestBuy = bestLimit.bestLimit.orderRow2.volumeBestBuy,
                    volumeBestSale = bestLimit.bestLimit.orderRow2.volumeBestSale
                },
                orderRow3 = new OrderRow()
                {
                    countBestBuy = bestLimit.bestLimit.orderRow3.countBestBuy,
                    countBestSale = bestLimit.bestLimit.orderRow3.countBestSale,
                    priceBestBuy = bestLimit.bestLimit.orderRow3.priceBestBuy,
                    priceBestSale = bestLimit.bestLimit.orderRow3.priceBestSale,
                    volumeBestBuy = bestLimit.bestLimit.orderRow3.volumeBestBuy,
                    volumeBestSale = bestLimit.bestLimit.orderRow3.volumeBestSale
                },
                orderRow4 = new OrderRow()
                {
                    countBestBuy = bestLimit.bestLimit.orderRow4.countBestBuy,
                    countBestSale = bestLimit.bestLimit.orderRow4.countBestSale,
                    priceBestBuy = bestLimit.bestLimit.orderRow4.priceBestBuy,
                    priceBestSale = bestLimit.bestLimit.orderRow4.priceBestSale,
                    volumeBestBuy = bestLimit.bestLimit.orderRow4.volumeBestBuy,
                    volumeBestSale = bestLimit.bestLimit.orderRow4.volumeBestSale
                },
                orderRow5 = new OrderRow()
                {
                    countBestBuy = bestLimit.bestLimit.orderRow5.countBestBuy,
                    countBestSale = bestLimit.bestLimit.orderRow5.countBestSale,
                    priceBestBuy = bestLimit.bestLimit.orderRow5.priceBestBuy,
                    priceBestSale = bestLimit.bestLimit.orderRow5.priceBestSale,
                    volumeBestBuy = bestLimit.bestLimit.orderRow5.volumeBestBuy,
                    volumeBestSale = bestLimit.bestLimit.orderRow5.volumeBestSale
                },
                orderRow6 = new OrderRow()
                {
                    countBestBuy = bestLimit.bestLimit.orderRow6.countBestBuy,
                    countBestSale = bestLimit.bestLimit.orderRow6.countBestSale,
                    priceBestBuy = bestLimit.bestLimit.orderRow6.priceBestBuy,
                    priceBestSale = bestLimit.bestLimit.orderRow6.priceBestSale,
                    volumeBestBuy = bestLimit.bestLimit.orderRow6.volumeBestBuy,
                    volumeBestSale = bestLimit.bestLimit.orderRow6.volumeBestSale
                },

            };
            var activeOrders = _externalOrderService.GetAllActives();

           
            foreach (var order in activeOrders.Model.Orders)
            {
               
                if (order.instrument.priceMax == result.orderRow1.priceBestBuy)
                        result.orderRow1.HasOrderBuy = true;

                 if(order.instrument.priceMax == result.orderRow2.priceBestBuy)
                        result.orderRow2.HasOrderBuy = true;

                 if (order.instrument.priceMax == result.orderRow3.priceBestBuy)
                    result.orderRow3.HasOrderBuy = true;

                 if (order.instrument.priceMax == result.orderRow4.priceBestBuy)
                    result.orderRow4.HasOrderBuy = true;

                 if (order.instrument.priceMax == result.orderRow5.priceBestBuy)
                    result.orderRow5.HasOrderBuy = true;

                 if (order.instrument.priceMax == result.orderRow6.priceBestBuy)
                    result.orderRow6.HasOrderBuy = true;

                if (order.instrument.priceMin == result.orderRow1.priceBestSale)
                        result.orderRow1.HasOrderSell = true;

                 if (order.instrument.priceMin == result.orderRow2.priceBestSale)
                    result.orderRow2.HasOrderSell = true;

                if(order.instrument.priceMin == result.orderRow3.priceBestSale)
                    result.orderRow3.HasOrderSell = true;

                 if (order.instrument.priceMin == result.orderRow4.priceBestSale)
                    result.orderRow4.HasOrderSell = true;

                if(order.instrument.priceMin == result.orderRow5.priceBestSale)
                    result.orderRow5.HasOrderSell = true;

                 if (order.instrument.priceMin == result.orderRow6.priceBestSale)
                    result.orderRow6.HasOrderSell = true;
                
            }

            var  proccessedResult = GetTotalvolume(result);

            
            return new ResultModel<Izi.Online.ViewModels.Instruments.BestLimit.BestLimits>(proccessedResult);
        }

        public ResultModel<InstrumentPriceDetails> Price(SelectInstrumentDetails model)
        {
            var result = HttpGetRequest<InstrumentPrice>($"rlc/price/{model.NscCode}");

            if (result.statusCode == -1)
                return new ResultModel<InstrumentPriceDetails>(result.price);

            return new ResultModel<InstrumentPriceDetails>(result.price, result.statusCode == 200, result.clientMessage, result.statusCode);
        }

        public ResultModel<Details> Details(SelectInstrumentDetails model)
        {
            var result = HttpGetRequest<InstrumentDetails>($"order/instrument/{model.InstrumentId}");


            if (result.statusCode == 200)
                return new ResultModel<Details>(result.Instrument);

            return new ResultModel<Details>(result.Instrument, result.statusCode == 200, result.clientMessage, result.statusCode);

        }
        public BestLimitsView GetTotalvolume(BestLimitsView bestLimits)
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

            bestLimits.orderRow1.QtyOrderBuy = bestLimits.orderRow1.priceBestBuy != 0 ? (int)PercentProccessor(totalBuys, bestLimits.orderRow1.priceBestBuy) : 0;
            bestLimits.orderRow2.QtyOrderBuy = bestLimits.orderRow2.priceBestBuy != 0 ? (int)PercentProccessor(totalBuys, bestLimits.orderRow2.priceBestBuy) : 0;
            bestLimits.orderRow3.QtyOrderBuy = bestLimits.orderRow3.priceBestBuy != 0 ? (int)PercentProccessor(totalBuys, bestLimits.orderRow3.priceBestBuy) : 0;
            bestLimits.orderRow4.QtyOrderBuy = bestLimits.orderRow4.priceBestBuy != 0 ? (int)PercentProccessor(totalBuys, bestLimits.orderRow4.priceBestBuy) : 0;
            bestLimits.orderRow5.QtyOrderBuy = bestLimits.orderRow5.priceBestBuy != 0 ? (int)PercentProccessor(totalBuys, bestLimits.orderRow5.priceBestBuy) : 0;
            bestLimits.orderRow6.QtyOrderBuy = bestLimits.orderRow6.priceBestBuy != 0 ? (int)PercentProccessor(totalBuys, bestLimits.orderRow6.priceBestBuy) : 0;

            bestLimits.orderRow1.QtyOrderSell = bestLimits.orderRow1.QtyOrderSell != 0 ? (int)PercentProccessor(totalSells, bestLimits.orderRow1.priceBestSale) : 0;
            bestLimits.orderRow2.QtyOrderSell = bestLimits.orderRow2.QtyOrderSell != 0 ? (int)PercentProccessor(totalSells, bestLimits.orderRow2.priceBestSale) : 0;
            bestLimits.orderRow3.QtyOrderSell = bestLimits.orderRow3.QtyOrderSell != 0 ? (int)PercentProccessor(totalSells, bestLimits.orderRow3.priceBestSale) : 0;
            bestLimits.orderRow4.QtyOrderSell = bestLimits.orderRow4.QtyOrderSell != 0 ? (int)PercentProccessor(totalSells, bestLimits.orderRow4.priceBestSale) : 0;
            bestLimits.orderRow5.QtyOrderSell = bestLimits.orderRow5.QtyOrderSell != 0 ? (int)PercentProccessor(totalSells, bestLimits.orderRow5.priceBestSale) : 0;
            bestLimits.orderRow6.QtyOrderSell = bestLimits.orderRow6.QtyOrderSell != 0 ? (int)PercentProccessor(totalSells, bestLimits.orderRow6.priceBestSale) : 0;

            return bestLimits;            
        }
        public double PercentProccessor(double a, double b)
        {

            return (a - b) / a * 100;
        }


    }
}
