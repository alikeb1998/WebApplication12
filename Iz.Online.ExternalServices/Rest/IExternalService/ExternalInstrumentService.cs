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

namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalInstrumentService : BaseService, IExternalInstrumentService
    {
        private readonly IInstrumentsRepository _instrumentsRepository;
        private readonly IPushService _pushService;
        public ExternalInstrumentService(IInstrumentsRepository instrumentsRepository, IPushService pushService) : base(instrumentsRepository)
        {
            _instrumentsRepository = instrumentsRepository;
            _pushService = pushService;
        }
        public bool UpdateInstrumentList(ViewBaseModel model)
        {
            var onDbInstrumentsList = _instrumentsRepository.GetInstrumentsList().Select(x => x.InstrumentId).ToList();
            var onDbInstrumentSector = _instrumentsRepository.GetInstrumentSector().Select(x => x.SectorId).ToList();
            var onDbInstrumentSubSectors = _instrumentsRepository.GetInstrumentSubSectors().Select(x => x.SubSectorId).ToList();
            var onDbInstrumentBourse = _instrumentsRepository.GetInstrumentBourse().Select(x => x.BourseId).ToList();

            var instruments = HttpGetRequest<Instruments>("order/instruments", model.Token);

            try
            {
                foreach (var instrument in instruments.instruments)
                {

                    if (!onDbInstrumentSector.Contains(instrument.sector.id))
                    {
                        _instrumentsRepository.AddInstrumentSector(instrument.sector);
                        onDbInstrumentSector.Add(instrument.sector.id);
                    }

                    if (!onDbInstrumentSubSectors.Contains(instrument.subSector.id))
                    {
                        _instrumentsRepository.AddInstrumentSubSectors(instrument.subSector);
                        onDbInstrumentSubSectors.Add(instrument.subSector.id);
                    }

                    if (!onDbInstrumentBourse.Contains(instrument.group.id))
                    {
                        _instrumentsRepository.AddInstrumentBourse(instrument.group);
                        onDbInstrumentBourse.Add(instrument.group.id);
                    }

                    if (!onDbInstrumentsList.Contains(instrument.Id))
                    {
                        _instrumentsRepository.AddInstrument(instrument);
                        onDbInstrumentsList.Add(instrument.Id);
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

        public Izi.Online.ViewModels.Instruments.BestLimit.BestLimits BestLimits(SelectedInstrument model)
        {
            //model.InstrumentId = "IRO1FOLD0001";

            Task.Run(async () => _pushService.ConsumeRefreshInstrumentBestLimit(model.NscCode));

            var result = HttpGetRequest<BestLimits>($"rlc/best-limit/{model.NscCode}", model.Token);
            if (result.bestLimit == null || result.statusCode != 200)
                return new Izi.Online.ViewModels.Instruments.BestLimit.BestLimits();
           
            return new Izi.Online.ViewModels.Instruments.BestLimit.BestLimits()
            {
                orderRow1 = new OrderRow()
                {
                    countBestBuy = result.bestLimit.orderRow1.countBestBuy ,
                    countBestSale = result.bestLimit.orderRow1.countBestSale,
                    priceBestBuy = result.bestLimit.orderRow1.priceBestBuy,
                    priceBestSale = result.bestLimit.orderRow1.priceBestSale,
                    volumeBestBuy = result.bestLimit.orderRow1.volumeBestBuy,
                    volumeBestSale = result.bestLimit.orderRow1.volumeBestSale
                },
                orderRow2 = new OrderRow()
                {
                    countBestBuy = result.bestLimit.orderRow2.countBestBuy,
                    countBestSale = result.bestLimit.orderRow2.countBestSale,
                    priceBestBuy = result.bestLimit.orderRow2.priceBestBuy,
                    priceBestSale = result.bestLimit.orderRow2.priceBestSale,
                    volumeBestBuy = result.bestLimit.orderRow2.volumeBestBuy,
                    volumeBestSale = result.bestLimit.orderRow2.volumeBestSale
                },
                orderRow3 = new OrderRow()
                {
                    countBestBuy = result.bestLimit.orderRow3.countBestBuy,
                    countBestSale = result.bestLimit.orderRow3.countBestSale,
                    priceBestBuy = result.bestLimit.orderRow3.priceBestBuy,
                    priceBestSale = result.bestLimit.orderRow3.priceBestSale,
                    volumeBestBuy = result.bestLimit.orderRow3.volumeBestBuy,
                    volumeBestSale = result.bestLimit.orderRow3.volumeBestSale
                },
                orderRow4 = new OrderRow()
                {
                    countBestBuy = result.bestLimit.orderRow4.countBestBuy,
                    countBestSale = result.bestLimit.orderRow4.countBestSale,
                    priceBestBuy = result.bestLimit.orderRow4.priceBestBuy,
                    priceBestSale = result.bestLimit.orderRow4.priceBestSale,
                    volumeBestBuy = result.bestLimit.orderRow4.volumeBestBuy,
                    volumeBestSale = result.bestLimit.orderRow4.volumeBestSale
                },
                orderRow5 = new OrderRow()
                {
                    countBestBuy = result.bestLimit.orderRow5.countBestBuy,
                    countBestSale = result.bestLimit.orderRow5.countBestSale,
                    priceBestBuy = result.bestLimit.orderRow5.priceBestBuy,
                    priceBestSale = result.bestLimit.orderRow5.priceBestSale,
                    volumeBestBuy = result.bestLimit.orderRow5.volumeBestBuy,
                    volumeBestSale = result.bestLimit.orderRow5.volumeBestSale
                },
                orderRow6 = new OrderRow()
                {
                    countBestBuy = result.bestLimit.orderRow6.countBestBuy,
                    countBestSale = result.bestLimit.orderRow6.countBestSale,
                    priceBestBuy = result.bestLimit.orderRow6.priceBestBuy,
                    priceBestSale = result.bestLimit.orderRow6.priceBestSale,
                    volumeBestBuy = result.bestLimit.orderRow6.volumeBestBuy,
                    volumeBestSale = result.bestLimit.orderRow6.volumeBestSale
                },

            };
        }

        public InstrumentPrice Price(SelectInstrumentDetails model)
        {
            var result = HttpGetRequest<InstrumentPrice>($"rlc/price/{model.NscCode}", model.Token);
            return result;
        }

        public InstrumentDetails Details(SelectInstrumentDetails model)
        {
            var result = HttpGetRequest<InstrumentDetails>($"order/instrument/{model.InstrumentId}", model.Token);
            return result;
        }

        //public InstrumentStates States(SelectedInstrument model)
        //{
        //    var result = HttpGetRequest<InstrumentStates>($"order/instrument/{model.InstrumentId}", model.Authorization);
        //    return result;
        //}
    }
}
