using Iz.Online.DataAccess;
using Iz.Online.Entities;
using Iz.Online.Reopsitory.IRepository;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchList = Izi.Online.ViewModels.Instruments.WatchList;

namespace Iz.Online.Reopsitory.Repository
{
    public class WatchListsRepository : BaseRepository, IWatchListsRepository
    {
        public WatchListsRepository(OnlineBackendDbContext dataBase) : base(dataBase)
        {
        }
        public ResultModel<List<WatchList>> GetUserWatchLists(string CustomerId)
        {
            try
            {
                var wl = _db.WathLists
                .Where(x => x.CustomerId == CustomerId)
                .Select(x => new WatchList()
                {
                    Id = x.Id,
                    WatchListName = x.WatchListName
                }).ToList();

                if (wl == null || wl.Count == 0)
                    return new ResultModel<List<WatchList>>(null, false, "دیده بان برای این مشتری تعریف نشده است", -1);

                return new ResultModel<List<WatchList>>(wl);

            }
            catch (Exception)
            {
                return new ResultModel<List<WatchList>>(null, false, "خطای پایگاه داده", -1);

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
                                Isin = x.Instrument.Isin,
                                //AskPrice = "10",
                                //BidPrice = "10",
                                //ChangePercent = 10.23f,
                                //ClosePrice = "10",
                                //LastPrice ="10",
                                Code = x.Instrument.Code

                            }).ToList()
                    }).FirstOrDefault();
                if (wl == null)
                    return new ResultModel<WatchListDetails>(null, false, "دیده بان یافت نشد", -1);

                return new ResultModel<WatchListDetails>(wl);

            }
            catch (Exception)
            {
                return new ResultModel<WatchListDetails>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public ResultModel<List<WatchList>> DeleteWatchList(SearchWatchList model)
        {
            try
            {
                var entity = _db.WathLists.FirstOrDefault(x => x.Id == model.WatchListId);

                if (entity == null)
                    return new ResultModel<List<WatchList>>(null, false, "دیده بان یافت نشد", -1);

                _db.WathLists.Remove(entity);
                _db.SaveChanges();

                return GetUserWatchLists(model.CustomerId);

            }
            catch (Exception)
            {
                return new ResultModel<List<WatchList>>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public ResultModel<WatchListDetails> NewWatchList(NewWatchList model)
        {
            try
            {
                var wlId = Guid.NewGuid().ToString();
                string query = $"INSERT  into WatchListsInstruments  values ";
                foreach (var id in model.Id)
                {
                    query += $" ({id},'{wlId}') ,";
                }
                query = query.Substring(0, query.Length - 1);

                _db.Database.ExecuteSqlRaw($"INSERT   INTO  WathLists VALUES ( '{wlId}'  , N'{model.WatchListName}','{model.CustomerId}');{query}");


                return GetWatchListDetails(new SearchWatchList()
                {
                    CustomerId = model.CustomerId,
                    WatchListId = wlId
                });
            }
            catch (Exception)
            {
                return new ResultModel<WatchListDetails>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public ResultModel<WatchListDetails> AddInstrumentToWatchList(EditEathListItems model)
        {
            try
            {

                _db.Database.ExecuteSqlRaw(
                    $"INSERT  into WatchListsInstruments (InstrumentId,WatchListId) values  ('{model.Id}','{model.WatchListId}')");

                return GetWatchListDetails(new SearchWatchList()
                {
                    CustomerId = model.CustomerId,
                    WatchListId = model.WatchListId
                });

            }
            catch (Exception)
            {
                return new ResultModel<WatchListDetails>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public ResultModel<WatchListDetails> RemoveInstrumentFromWatchList(EditEathListItems model)
        {
            try
            {
                _db.Database.ExecuteSqlRaw(
                $"DELETE FROM WatchListsInstruments WHERE InstrumentId={model.Id} AND WatchListId='{model.WatchListId}'");

                return GetWatchListDetails(new SearchWatchList()
                {
                    CustomerId = model.CustomerId,
                    WatchListId = model.WatchListId
                });
            }
            catch (Exception)
            {
                return new ResultModel<WatchListDetails>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public ResultModel<List<WatchList>> InstrumentWatchLists(InstrumentWatchLists model)
        {
            try
            {

                var wl = _db.WathLists
                     .Where(w => w.CustomerId == model.CustomerId && w.WatchListsInstruments.Select(x => x.InstrumentId).Contains(model.Id))
                     .SelectMany(c => c.WatchListsInstruments, (c, w) =>
                          new WatchList
                          {
                              WatchListName = w.WatchList.WatchListName,
                              Id = w.WatchList.Id
                          }).Distinct().ToList();

                if (wl == null || wl.Count == 0)
                    return new ResultModel<List<WatchList>>(null, false, "دیده بان یافت نشد", -1);

                return new ResultModel<List<WatchList>>(wl);

            }
            catch (Exception e)
            {
                return new ResultModel<List<WatchList>>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public ResultModel<WatchListDetails> UpdateWatchList(EditWatchList model)
        {

            var entity = _db.WathLists.FirstOrDefault(x => x.Id == model.WatchListId);

            entity.WatchListName = model.WatchListName;
            _db.Database.ExecuteSqlRaw(@$"delete from WatchListsInstruments where WatchListId='{model.Id}'");
            _db.SaveChanges();
            string query = $"INSERT  into WatchListsInstruments  values ";
            foreach (var id in model.Id)
            {
                query += $" ({id},'{model.Id}') ,";
            }
            query = query.Substring(0, query.Length - 1);

            _db.Database.ExecuteSqlRaw(query);

            return GetWatchListDetails(new SearchWatchList()
            {
                CustomerId = model.CustomerId,
                WatchListId = model.WatchListId
            });
        }

    }
}
