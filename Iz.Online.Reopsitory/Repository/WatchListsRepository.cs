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
        public async Task<ResultModel<List<WatchList>>> GetUserWatchLists(string CustomerId)
        {
            try
            {
                var wl = await _db.WathLists
                .Where(x => x.CustomerId == CustomerId)
                .Select(x => new WatchList()
                {
                    Id = x.Id,
                    WatchListName = x.WatchListName,
                    
                }).ToListAsync();

                if (wl == null || wl.Count == 0)
                    return new ResultModel<List<WatchList>>(null, true, "دیده بان برای این مشتری تعریف نشده است", -1);

                return new ResultModel<List<WatchList>>(wl);

            }
            catch (Exception)
            {
                return new ResultModel<List<WatchList>>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<WatchListDetails>> GetWatchListDetails(SearchWatchList model)
        {
            try
            {

                var wl = await _db.WathLists
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
                    }).FirstOrDefaultAsync();
                if (wl == null)
                    return new ResultModel<WatchListDetails>(null, true, "دیده بان یافت نشد", -1);

                return new ResultModel<WatchListDetails>(wl);

            }
            catch (Exception)
            {
                return new ResultModel<WatchListDetails>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<List<WatchList>>> DeleteWatchList(SearchWatchList model)
        {
            try
            {
                var entity = _db.WathLists.FirstOrDefault(x => x.Id == model.WatchListId);

                if (entity == null)
                    return new ResultModel<List<WatchList>>(null, false, "دیده بان یافت نشد", -1);

                _db.WathLists.Remove(entity);
                await _db.SaveChangesAsync();

                return await GetUserWatchLists(model.TradingId);

            }
            catch (Exception)
            {
                return new ResultModel<List<WatchList>>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<WatchListDetails>> NewWatchList(NewWatchList model)
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

                var c = $"INSERT   INTO  WathLists VALUES ( '{wlId}'  , N'{model.WatchListName}','{model.TradingId}')";

                 await _db.Database.ExecuteSqlRawAsync($"INSERT   INTO  WathLists VALUES ( '{wlId}'  , N'{model.WatchListName}','{model.TradingId}');{query}");


                return await GetWatchListDetails(new SearchWatchList()
                {
                    TradingId = model.TradingId,
                    WatchListId = wlId
                });
            }
            catch (Exception)
            {
                return new ResultModel<WatchListDetails>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<WatchListDetails>> AddInstrumentToWatchList(EditEathListItems model)
        {
            try
            {

                 await _db.Database.ExecuteSqlRawAsync(
                    $"INSERT  into WatchListsInstruments (InstrumentId,WatchListId) values  ('{model.Id}','{model.WatchListId}')");

                return await GetWatchListDetails(new SearchWatchList()
                {
                    TradingId = model.TradingId,
                    WatchListId = model.WatchListId
                });

            }
            catch (Exception)
            {
                return new ResultModel<WatchListDetails>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<WatchListDetails>> RemoveInstrumentFromWatchList(EditEathListItems model)
        {
            try
            {
                await _db.Database.ExecuteSqlRawAsync(
                $"DELETE FROM WatchListsInstruments WHERE InstrumentId={model.Id} AND WatchListId='{model.WatchListId}'");

                return await GetWatchListDetails(new SearchWatchList()
                {
                    TradingId = model.TradingId,
                    WatchListId = model.WatchListId
                });
            }
            catch (Exception)
            {
                return new ResultModel<WatchListDetails>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<List<WatchList>>> InstrumentWatchLists(InstrumentWatchLists model)
        {
            try
            {

                var wl = await _db.WathLists
                     .Where(w => w.CustomerId == model.TradingId && w.WatchListsInstruments.Select(x => x.InstrumentId).Contains(model.Id))
                     .SelectMany(c => c.WatchListsInstruments, (c, w) =>
                          new WatchList
                          {
                              WatchListName = w.WatchList.WatchListName,
                              Id = w.WatchList.Id
                          }).Distinct().ToListAsync();

                if (wl == null || wl.Count == 0)
                    return new ResultModel<List<WatchList>>(null, true, "دیده بان یافت نشد", -1);

                return new ResultModel<List<WatchList>>(wl);

            }
            catch (Exception e)
            {
                return new ResultModel<List<WatchList>>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<WatchListDetails>> UpdateWatchList(EditWatchList model)
        {
            
            var entity = _db.WathLists.FirstOrDefault(x => x.Id == model.WatchListId);

            entity.WatchListName = model.WatchListName;
            
           // _db.Database.ExecuteSqlRaw(@$"delete from WatchListsInstruments where WatchListId='{model.Id}'");
             _db.SaveChanges();
            string query = $"INSERT  into WatchListsInstruments  values ";
            foreach (var id in model.Id)
            {
                _db.Database.ExecuteSqlRaw(@$"delete from WatchListsInstruments where WatchListId='{model.WatchListId}'");
                await _db.SaveChangesAsync();
                query += $" ({id},'{model.WatchListId}') ,";
            }
            query = query.Substring(0, query.Length - 1);

            await _db.Database.ExecuteSqlRawAsync(query);

            return await GetWatchListDetails(new SearchWatchList()
            {
                TradingId = model.TradingId,
                WatchListId = model.WatchListId
            });
        }

    }
}
