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
            // var list = _instrumentsRepository.GetInstrumentsList().Where(x=>x.Isin.LastIndexOf('1') == x.Isin.Length).ToList();
            var list = _instrumentsRepository.GetInstrumentsList().Select(x => new InstrumentList()
            {
                Id = x.Id,
                Name = $"{x.SymbolName} ({x.CompanyName}) {x.Bourse}",
                NscCode = x.Isin
            }).ToList();
            var res = new List<InstrumentList>();  

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].NscCode.EndsWith('1'))
                {
                    if (list[i].Name.EndsWith('1') || list[i].Name.EndsWith('3'))
                    {
                        var name = list[i].Name.Substring(0, list[i].Name.Length-1);
                        list[i].Name = name;
                        res.Add(list[i]);
                    }
                }
            }
           
            return res;
           
          
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
