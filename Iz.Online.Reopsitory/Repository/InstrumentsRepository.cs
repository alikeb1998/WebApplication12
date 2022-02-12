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

        public ResultModel<List<Instruments>> GetInstrumentsList()
        {
            try
            {
                var ins = _db.Instruments.Select(x => new Instruments()
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
                    InstrumentId = x.InstrumentId,

                }).ToList();
                return new ResultModel<List<Instruments>>(ins);
            }
            catch (Exception)
            {
                return new ResultModel<List<Instruments>>(null, false);

            }
        }

        public ResultModel<List<InstrumentBourse>> GetInstrumentBourse()
        {
            try
            {
                var result = _db.InstrumentBourses.ToList();
                return new ResultModel<List<InstrumentBourse>>(result);

            }
            catch (Exception)
            {
                return new ResultModel<List<InstrumentBourse>>(null, false);

            }
        }

        public ResultModel<List<InstrumentSector>> GetInstrumentSector()
        {
            try
            {

                return new ResultModel<List<InstrumentSector>>(_db.InstrumentSectors.ToList(), false);
            }

            catch (Exception)
            {
                return new ResultModel<List<InstrumentSector>>(null, false);

            }
        }

        public ResultModel<List<InstrumentSubSector>> GetInstrumentSubSectors()
        {
            try
            {

                return new ResultModel<List<InstrumentSubSector>>(_db.InstrumentSubSectors.ToList(), true);
            }

            catch (Exception)
            {
                return new ResultModel<List<InstrumentSubSector>>(null, false);

            }
        }

        public ResultModel<bool> AddInstrumentBourse(InstrumentBourse model)
        {
            try
            {

                _db.InstrumentBourses.Add(model);
                _db.SaveChanges();
                return new ResultModel<bool>(true);

            }

            catch (Exception)
            {
                return new ResultModel<bool>(false, false);

            }
        }

        public ResultModel<bool> AddInstrumentSector(InstrumentSector model)
        {
            try
            {
                _db.InstrumentSectors.Add(model);
                _db.SaveChanges();
                return new ResultModel<bool>(true);

            }
            catch (Exception)
            {
                return new ResultModel<bool>(false, false);

            }
        }

        public ResultModel<bool> AddInstrumentSubSectors(InstrumentSubSector model)
        {
            try
            {
                _db.InstrumentSubSectors.Add(model);
                _db.SaveChanges();
                return new ResultModel<bool>(true);

            }
            catch (Exception)
            {
                return new ResultModel<bool>(false, false);

            }
        }

        public ResultModel<bool> AddInstrument(Instrument model)
        {
            try

            {
                _db.Instruments.Add(model);
                _db.SaveChanges();
                return new ResultModel<bool>(true);

            }
            catch (Exception)
            {
                return new ResultModel<bool>(false, false);

            }
        }

        public ResultModel<List<WatchList>> GetUserWatchLists(ViewBaseModel model)
        {
            try
            {
                var wl = _db.WathLists
                .Where(x => x.CustomerId == model.CustomerId)
                .Select(x => new WatchList()
                {
                    Id = x.Id,
                    WatchListName = x.WatchListName
                }).ToList();

                return new ResultModel<List<WatchList>>(wl, false);

            }
            catch (Exception)
            {
                return new ResultModel<List<WatchList>>(null, false);

            }
        }

        public ResultModel<WatchListDetails> GetWatchListDetails(SearchWatchList model)
        {
            try
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

                return new ResultModel<WatchListDetails>(wl);

            }
            catch (Exception)
            {
                return new ResultModel<WatchListDetails>(null, false);

            }
        }

        public ResultModel<List<WatchList>> DeleteWatchList(SearchWatchList model)
        {
            try
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
            catch (Exception)
            {
                return new ResultModel<List<WatchList>>(null, false);

            }
        }

        public ResultModel<WatchListDetails> NewWatchList(NewWatchList model)
        {
            try
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
            catch (Exception)
            {
                return new ResultModel<WatchListDetails>(null, false);

            }
        }

        public ResultModel<WatchListDetails> AddInstrumentToWatchList(EditEathListItems model)
        {
            try
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
            catch (Exception)
            {
                return new ResultModel<WatchListDetails>(null, false);

            }
        }

        public ResultModel<WatchListDetails> RemoveInstrumentFromWatchList(EditEathListItems model)
        {
            try
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
            catch (Exception)
            {
                return new ResultModel<WatchListDetails>(null, false);

            }
        }

        public ResultModel<List<WatchList>> InstrumentWatchLists(InstrumentWatchLists model)
        {
            try
            {
                var wl = _db.WathLists
                    .Where(w => w.CustomerId == model.CustomerId).Distinct()
                    .SelectMany(c => c.WatchListsInstruments, (c, w) =>
                         new WatchList
                         {
                             WatchListName = w.WatchList.WatchListName,
                             Id = w.WatchList.Id
                         }).Distinct().ToList();

                return new ResultModel<List<WatchList>>(wl, true);

            }
            catch (Exception)
            {
                return new ResultModel<List<WatchList>>(null, false);

            }
        }



    }
}
