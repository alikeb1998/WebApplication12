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

        public List<Instruments> Instruments()
        {
            return _instrumentsRepository.GetInstrumentsList();
        }
        public List<InstrumentList> InstrumentList()
        {
            return _instrumentsRepository.GetInstrumentsList()
                .Select(x =>  new InstrumentList()
            {
                Id =x.Id,
                Name = $"{x.SymbolName} ({x.CompanyName}) {x.Bourse}",
                NscCode = x.Isin
            }).ToList();
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
    }

}
