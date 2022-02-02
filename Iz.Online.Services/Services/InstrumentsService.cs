using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;

namespace Iz.Online.Services.Services
{
    public class InstrumentsService : IInstrumentsService
    {
        public IInstrumentsRepository _instrumentsRepository { get; set; }

        public InstrumentsService(IInstrumentsRepository instrumentsRepository)
        {
            _instrumentsRepository = instrumentsRepository;
        }

        public List<Instruments> Instruments(string token)
        {
            return _instrumentsRepository.GetInstrumentsList();
        }
        public List<InstrumentList> InstrumentList(string token)
        {
            return _instrumentsRepository.GetInstrumentsList()
                .Select(x =>  new InstrumentList()
            {
                Id =x.Id,
                Name = $"{x.SymbolName} ({x.CompanyName}) {x.Bourse}"
            }).ToList();
        }

        public List<WatchList> UserWatchLists(ViewBaseModel model, string token)
        {
            return _instrumentsRepository.GetUserWatchLists(model);
        }

        public WatchListDetails WatchListDetails(SearchWatchList model, string token)
        {
            return _instrumentsRepository.GetWatchListDetails(model);
        }


        public List<WatchList> DeleteWatchList(SearchWatchList model, string token)
        {
            return _instrumentsRepository.DeleteWatchList(model);
        }

        public WatchListDetails NewWatchList(NewWatchList model, string token)
        {
            return _instrumentsRepository.NewWatchList(model);
        }

        public WatchListDetails AddInstrumentToWatchList(EditEathListItems model, string token)
        {
            return _instrumentsRepository.AddInstrumentToWatchList(model);

        }

        public WatchListDetails RemoveInstrumentFromWatchList(EditEathListItems model, string token)
        {
            return _instrumentsRepository.RemoveInstrumentFromWatchList(model);

        }

        public List<WatchList> InstrumentWatchLists(InstrumentWatchLists model, string token)
        {
            return _instrumentsRepository.InstrumentWatchLists(model);

        }
    }

}
