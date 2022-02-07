using Iz.Online.Reopsitory.IRepository;
using Iz.Online.DataAccess;
using Iz.Online.Entities;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.EntityFrameworkCore;
using WatchList = Izi.Online.ViewModels.Instruments.WatchList;
using System.Linq;

namespace Iz.Online.Reopsitory.Repository
{
    public class InstrumentsRepository : BaseRepository, IInstrumentsRepository
    {
        public InstrumentsRepository(OnlineBackendDbContext dataBase) : base(dataBase)
        {
        }

        public List<Instruments> GetInstrumentsList()
        {
           // var list = _db.Instruments.Where(x => x.Isin.LastIndexOf('1') == x.Isin.Length).ToList();

            var ins = _db.Instruments
                .Select(x => new Instruments()
                {
                    CompanyName = x.CompanyName,
                    Id = x.Id,
                    Isin = x.Isin,
                    SymbolName = x.SymbolName,
                    BaseVolume = x.BaseVolume,
                    Code = x.Code,
                    Bourse = x.Bourse.borse,
                    Sector = x.Sector.Name,
                    SubSector = x.SubSector.Name,

                }).ToList();
            

            return ins;
        }

        public List<InstrumentBourse> GetInstrumentBourse()
        {
            return _db.InstrumentBourses.ToList();
        }

        public List<InstrumentSector> GetInstrumentSector()
        {
            return _db.InstrumentSectors.ToList();
        }

        public List<InstrumentSubSector> GetInstrumentSubSectors()
        {
            return _db.InstrumentSubSectors.ToList();
        }

        public void AddInstrumentBourse(InstrumentBourse model)
        {
            _db.InstrumentBourses.Add(model);
            _db.SaveChanges();
        }

        public void AddInstrumentSector(InstrumentSector model)
        {
            _db.InstrumentSectors.Add(model);
            _db.SaveChanges();
        }

        public void AddInstrumentSubSectors(InstrumentSubSector model)
        {
            _db.InstrumentSubSectors.Add(model);
            _db.SaveChanges();
        }

        public void AddInstrument(Instrument model)
        {
            _db.Instruments.Add(model);
            _db.SaveChanges();
        }

        public List<WatchList> GetUserWatchLists(ViewBaseModel model)
        {
            var wl = _db.WathLists
                .Where(x => x.CustomerId == model.CustomerId)
                .Select(x => new WatchList()
                {
                    Id = x.Id,
                    WatchListName = x.WatchListName
                }).ToList();

            return wl;
        }

        public WatchListDetails GetWatchListDetails(SearchWatchList model)
        {
            var wl = _db.WathLists
                .Where(x => x.Id == model.WatchListId)
                .Select(x => new WatchListDetails
                {
                    WatchList = new WatchList()
                    {
                        Id = x.Id,
                        WatchListName = x.WatchListName
                    },
                    Instruments = x.WatchListsInstruments
                        .Select(x => new Instruments
                        {
                            CompanyName = x.Instrument.CompanyName,
                            Id = x.Instrument.Id,
                            SymbolName = x.Instrument.SymbolName,
                            Isin = x.Instrument.Isin

                        }).ToList()
                }).FirstOrDefault();

            return wl;
        }

        public List<WatchList> DeleteWatchList(SearchWatchList model)
        {
            var entity = _db.WathLists.FirstOrDefault(x => x.Id == model.WatchListId);
            _db.WathLists.Remove(entity);
            _db.SaveChanges();

            return GetUserWatchLists(new ViewBaseModel()
            {
                CustomerId = model.CustomerId,
                Token = model.Token
            });

        }

        public WatchListDetails NewWatchList(NewWatchList model)
        {
            var wlId = Guid.NewGuid().ToString();
            string query = $"INSERT  into WatchListsInstruments  values ";
            foreach (var id in model.InstrumentsId)
            {
                query += $" ({id},'{wlId}') ,";
            }
            query = query.Substring(0, query.Length - 1);

            _db.Database.ExecuteSqlRaw($"INSERT   INTO  WathLists VALUES ( '{wlId}'  , N'{model.WatchListName}','{model.CustomerId}');{query}");


            return GetWatchListDetails(new SearchWatchList()
            {
                CustomerId = model.CustomerId,
                Token = model.Token,
                WatchListId = wlId
            });
        }

        public WatchListDetails AddInstrumentToWatchList(EditEathListItems model)
        {
            _db.Database.ExecuteSqlRaw(
                $"INSERT  into WatchListsInstruments (InstrumentId,WatchListId) values  ('{model.InstrumentsId}','{model.WatchListId}')");

            return GetWatchListDetails(new SearchWatchList()
            {
                CustomerId = model.CustomerId,
                Token = model.Token,
                WatchListId = model.WatchListId
            });

        }

        public WatchListDetails RemoveInstrumentFromWatchList(EditEathListItems model)
        {
            _db.Database.ExecuteSqlRaw(
                $"DELETE FROM WatchListsInstruments WHERE InstrumentId='{model.InstrumentsId}' AND WatchListId='{model.WatchListId}'");

            return GetWatchListDetails(new SearchWatchList()
            {
                CustomerId = model.CustomerId,
                Token = model.Token,
                WatchListId = model.WatchListId
            });
        }

        public List<WatchList> InstrumentWatchLists(InstrumentWatchLists model)
        {
            var wl = _db.WathLists
                .Where(w => w.CustomerId == model.CustomerId).Distinct()
                .SelectMany(c => c.WatchListsInstruments, (c, w) =>
                     new WatchList
                     {
                         WatchListName = w.WatchList.WatchListName,
                         Id = w.WatchList.Id
                     }).Distinct().ToList();
            return wl;
        }



    }
}
