using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using InstrumentStates = Izi.Online.ViewModels.Instruments.InstrumentStates;
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
                Bourse = x.Bourse
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

        public InstrumentStates States(Instrument model)
        {
            var respond = _IExternalInstrumentsService.States(model);

            return new InstrumentStates()
            {
                State = respond.Instrument.State,
                GroupState = respond.Instrument.Group.State,
            };
        }
    }

}
