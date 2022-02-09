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

        public List<Instruments> Instruments()
        {
            return _instrumentsRepository.GetInstrumentsList();
        }
        public List<InstrumentList> InstrumentList()
        {
            // var list = _instrumentsRepository.GetInstrumentsList().Where(x=>x.Isin.LastIndexOf('1') == x.Isin.Length).ToList();
            var list = _instrumentsRepository.GetInstrumentsList().Select(x => new InstrumentList()
            {
                Id = x.Id,
                Name = x.SymbolName.Substring(0,x.SymbolName.Length-1),
                FullName = x.CompanyName,
                NscCode = x.Code,
                Bourse = x.Bourse,
                InstrumentId = x.InstrumentId
            }).ToList();
                      
            return list;
           
        }

        public List<WatchList> UserWatchLists(ViewBaseModel model)
        {
            return _instrumentsRepository.GetUserWatchLists(model);
        }

        public WatchListDetails WatchListDetails(SearchWatchList model)
        {
            return _instrumentsRepository.GetWatchListDetails(model);
        }


        public List<WatchList> DeleteWatchList(SearchWatchList model)
        {
            return _instrumentsRepository.DeleteWatchList(model);
        }

        public WatchListDetails NewWatchList(NewWatchList model)
        {
            return _instrumentsRepository.NewWatchList(model);
        }

        public WatchListDetails AddInstrumentToWatchList(EditEathListItems model)
        {
            return _instrumentsRepository.AddInstrumentToWatchList(model);

        }

        public WatchListDetails RemoveInstrumentFromWatchList(EditEathListItems model)
        {
            return _instrumentsRepository.RemoveInstrumentFromWatchList(model);

        }

        public List<WatchList> InstrumentWatchLists(InstrumentWatchLists model)
        {
            return _instrumentsRepository.InstrumentWatchLists(model);

        }

        //public InstrumentPrice Price(Instrument model)
        //{
        //    var respond = _IExternalInstrumentsService.Price(model);

        //    return new InstrumentPrice() { };

        //}

        public InstrumentDetail Detail(Instrument model)
        {
            var detail = _IExternalInstrumentsService.Details(model);
            var priceDetail = _IExternalInstrumentsService.Price(model);

            var res = new InstrumentDetail()
            {
                State = detail.Instrument.State,
                GroupState = detail.Instrument.Group.State,
                closingPrice = priceDetail.price.closingPrice,
                firstPrice = priceDetail.price.firstPrice,
                lastPrice = priceDetail.price.lastPrice,
                instrumentId = priceDetail.price.instrumentId,
                lastTradeDate = priceDetail.price.lastTradeDate,
                valueOfTrades = priceDetail.price.valueOfTrades,
                numberOfTrades = Convert.ToInt32(priceDetail.price.numberOfTrades),
                volumeOfTrades = Convert.ToInt32(priceDetail.price.volumeOfTrades),
                yesterdayPrice = priceDetail.price.yesterdayPrice,
                PriceMax = detail.Instrument.PriceMax,
                PriceMin = detail.Instrument.PriceMin,
                highPrice = priceDetail.price.maximumPrice,
                lowPrice = priceDetail.price.minimumPrice,
            
            };
            return res;
        }

    }

}
