using Iz.Online.Reopsitory.IRepository;
using Iz.Online.DataAccess;
using Iz.Online.Entities;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Izi.Online.ViewModels.Users;
using Persons = Izi.Online.ViewModels.SignalR.Persons;
using Izi.Online.ViewModels.SignalR;

namespace Iz.Online.Reopsitory.Repository
{
    public class InstrumentsRepository : BaseRepository, IInstrumentsRepository
    {
        private readonly IServer _redis;
        private readonly IDistributedCache _cache;
        public InstrumentsRepository(OnlineBackendDbContext dataBase, IConnectionMultiplexer redis, IDistributedCache cache, IConfiguration configuration) : base(dataBase)
        {
            IConfiguration _configuration = configuration;
            var redisConnection = _configuration.GetSection("RedisConnection").Get<string>();
            _redis = redis.GetServer(redisConnection);

            _cache = cache;
        }

        public async Task<ResultModel<List<Instruments>>> GetInstrumentsList()
        {
            try
            {
                var ins =  await  _db.Instruments.Select(x => new Instruments()
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
                    Tick = x.Tick,
                    BuyCommisionRate = x.BuyCommisionRate,
                    SellCommisionRate = x.SellCommisionRate,

                }).ToListAsync();

                return new ResultModel<List<Instruments>>(ins);
            }
            catch (Exception e)
            {
                return new ResultModel<List<Instruments>>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<List<InstrumentBourse>>> GetInstrumentBourse()
        {
            try
            {
                var result =  await _db.InstrumentBourses.ToListAsync();
                return new ResultModel<List<InstrumentBourse>>(result);

            }
            catch (Exception)
            {
                return new ResultModel<List<InstrumentBourse>>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<List<InstrumentSector>>> GetInstrumentSector()
        {
            try
            {

                return new ResultModel<List<InstrumentSector>>(await _db.InstrumentSectors.ToListAsync());
            }

            catch (Exception)
            {
                return new ResultModel<List<InstrumentSector>>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<List<InstrumentSubSector>>> GetInstrumentSubSectors()
        {
            try
            {

                return new ResultModel<List<InstrumentSubSector>>(await _db.InstrumentSubSectors.ToListAsync());
            }

            catch (Exception)
            {
                return new ResultModel<List<InstrumentSubSector>>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<bool>> AddInstrumentBourse(InstrumentBourse model)
        {
            try
            {

                _db.InstrumentBourses.Add(model);
                await _db.SaveChangesAsync();
                return new ResultModel<bool>(true);

            }

            catch (Exception)
            {
                return new ResultModel<bool>(false, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<bool>> AddInstrumentSector(InstrumentSector model)
        {
            try
            {
                _db.InstrumentSectors.Add(model);
               await _db.SaveChangesAsync();
                return new ResultModel<bool>(true);

            }
            catch (Exception)
            {
                return new ResultModel<bool>(false, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<bool>> AddInstrumentSubSectors(InstrumentSubSector model)
        {
            try
            {
                _db.InstrumentSubSectors.Add(model);
                await _db.SaveChangesAsync();
                return new ResultModel<bool>(true);

            }
            catch (Exception)
            {
                return new ResultModel<bool>(false, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<bool>> AddInstrument(Instrument model, int sectorId, int subSectorId, int bourseId, long tick, float BuyCommissionRate, float SellCommissionRate)
        {
            try

            {
                model.SubSectorId = subSectorId;
                model.SectorId = sectorId;
                model.BourseId = bourseId;

                _db.Instruments.Add(model);
                await _db.SaveChangesAsync();
                return new ResultModel<bool>(true);

            }
            catch (Exception)
            {
                return new ResultModel<bool>(false, false, "خطای پایگاه داده", -1);

            }
        }
        public async Task<ResultModel<bool>> UpdateInstruments(Instrument model, int sectorId, int subSectorId, int bourseId, long tick, float BuyCommissionRate, float SellCommissionRate)
        {
            try
            {
                var entity =  _db.Instruments.FirstOrDefault(x => x.InstrumentId == model.InstrumentId);


                entity.SubSectorId = subSectorId;
                entity.SectorId = sectorId;
                entity.BourseId = bourseId;
                entity.Tick = tick;
                //////this numbers got from sattar.
                entity.BuyCommisionRate = BuyCommissionRate;
                entity.SellCommisionRate = SellCommissionRate;
                //////
                await _db.SaveChangesAsync();
                return new ResultModel<bool>(true);

            }
            catch (Exception e)
            {
                LogException(e);
                return new ResultModel<bool>(false, false, "خطای پایگاه داده", -1);

            }
        }


        public async Task<ResultModel<bool>> AddCommentToInstrument(AddCommentForInstrument model)
        {
            try
            {
                var entity = _db.InstrumentComments.FirstOrDefault(x =>
                    x.CustomerId == model.TradingId && x.InstrumentId == model.InstrumentId);

                if (entity != null)
                {
                    entity.CommentText = model.Comment;
                    await _db.SaveChangesAsync();
                    return new ResultModel<bool>(true);
                }
                else
                {
                    _db.InstrumentComments.Add(new InstrumentComment()
                    {
                        InstrumentId = model.InstrumentId,
                        CommentText = model.Comment,
                        Id = Guid.NewGuid().ToString(),
                        CustomerId = model.TradingId
                    });
                    await _db.SaveChangesAsync();
                }

                return new ResultModel<bool>(true);
            }
            catch (Exception e)
            {
                return new ResultModel<bool>(false, false, "خطای پایگاه داده", -1);

            }
        }

        public async Task<ResultModel<string>> GetInstrumentComment(GetInstrumentComment model)
        {
            var entity = await _db.InstrumentComments.FirstOrDefaultAsync(x =>
                x.CustomerId == model.TradingId && x.InstrumentId == model.InstrumentId);

            if (entity != null)
                return new ResultModel<string>(entity.CommentText);

            return new ResultModel<string>(null, true, "یادداشتی ثبت نشده است", -1);
        }


        #region instrument

        public void CleareCache()
        {
            var allInstrument = _redis.Keys(pattern: "Instrument*");

            foreach (var instrument in allInstrument)
            {
                _cache.Remove(instrument);

            }
        }
        public  List<InstrumentList> InstrumentData()
        {
            try
            {
                // _cache.Remove("");
                
                var allInstrument = _redis.Keys(pattern: "Instrument*");
                List<InstrumentList> result = new List<InstrumentList>();
                foreach (var instrument in allInstrument)
                {
                    var data =  _cache.Get(instrument);
                    var ins = JsonConvert.DeserializeObject<InstrumentList>(Encoding.Default.GetString(data));
                    result.Add(ins);
                }

                if (result.Count > 0)
                    return result;

                result = SqlInstrumentList();
                CacheInstrumentsData();
                return result;
            }
            catch (Exception e)
            {

                var result = SqlInstrumentList();
                CacheInstrumentsData();
                return result;
            }
        }

        public  InstrumentList InstrumentData(int instrumentId)
        {
            try
            {
                var dataBytes = _cache.Get("Instrument" + instrumentId);
                if (dataBytes == null)
                    CacheInstrumentsData();

                dataBytes =  _cache.Get("Instrument" + instrumentId);
                var result = JsonConvert.DeserializeObject<InstrumentList>(Encoding.Default.GetString(dataBytes));
                if (result != null)
                    return result;

                result = SqlInstrumentList().FirstOrDefault(x => x.Id == instrumentId);
                CacheInstrumentsData();
                return result;
            }
            catch (Exception e)
            {

                var result = SqlInstrumentList().FirstOrDefault(x => x.Id == instrumentId);
                CacheInstrumentsData();
                return result;
            }
        }
        public InstrumentList InstrumentData(string nsc)
        {
            try
            {
                var dataBytes = _cache.Get("Instrument" + nsc);
                if (dataBytes == null)
                    CacheInstrumentsData();

                dataBytes = _cache.Get("Instrument" + nsc);
                var result = JsonConvert.DeserializeObject<InstrumentList>(Encoding.Default.GetString(dataBytes));
                if (result != null)
                    return result;

                result = SqlInstrumentList().FirstOrDefault(x => x.NscCode == nsc);
                CacheInstrumentsData();
                return result;
            }
            catch (Exception e)
            {

                var result = SqlInstrumentList().FirstOrDefault(x => x.NscCode == nsc);
                CacheInstrumentsData();
                return result;
            }
        }
        public int GetLocalInstrumentIdFromOmsId(int omsId)
        {
            try
            {
                var dataBytes = _cache.Get("omsId" + omsId); 
                //var dataBytes = _cache.Get("omsId" + omsId);
                if (dataBytes == null)
                   CacheInstrumentsData();

                dataBytes = _cache.Get("omsId" + omsId);
                var result = JsonConvert.DeserializeObject<int>(Encoding.Default.GetString(dataBytes));
                if (result != null)
                    return result;

                result = (int)SqlInstrumentList().FirstOrDefault(x => x.InstrumentId == omsId).Id;
                CacheInstrumentsData();
                return result;
            }
            catch (Exception e)
            {
                var result = (int)SqlInstrumentList().FirstOrDefault(x => x.InstrumentId == omsId).Id;

                CacheInstrumentsData();
                return result;
            }
        }
        public int GetOmsIdFromLocalInstrumentId(int id)
        {
            try
            {
                var dataBytes = _cache.Get("InstrumentId" + id);
                if (dataBytes == null)
                    CacheInstrumentsData();

                dataBytes = _cache.Get("InstrumentId" + id);
                var result = JsonConvert.DeserializeObject<int>(Encoding.Default.GetString(dataBytes));
                if (result != null)
                    return result;

                result = (int)SqlInstrumentList().FirstOrDefault(x => x.Id == id).InstrumentId;
                CacheInstrumentsData();
                return result;
            }
            catch (Exception e)
            {
                var result = (int)SqlInstrumentList().FirstOrDefault(x => x.Id == id).InstrumentId;

                CacheInstrumentsData();
                return result;
            }
        }

        private List<InstrumentList> SqlInstrumentList()
        {
            var result = _db.Instruments.Select(instrument => new InstrumentList()
            {
                Id = instrument.Id,
                Name = instrument.SymbolName.EndsWith("1")
                    ? instrument.SymbolName.Substring(0, instrument.SymbolName.Length - 1)
                    : instrument.SymbolName,
                FullName = instrument.CompanyName,
                NscCode = instrument.Code, //
                Bourse = instrument.BourseId.Value, //
                InstrumentId = instrument.InstrumentId, //
                Tick = instrument.Tick,
                BuyCommissionRate = instrument.BuyCommisionRate,
                SellCommissionRate = instrument.SellCommisionRate,
            }).ToList();
            return result;
        }

        private bool CacheInstrumentsData()
        {
            try
            {

                var instruments = _db.Instruments.ToList();
                foreach (var instrument in instruments)
                {
                    var serializedData = JsonConvert.SerializeObject(new InstrumentList()
                    {
                        Id = instrument.Id,
                        Name = instrument.SymbolName.EndsWith("1") ? instrument.SymbolName.Substring(0, instrument.SymbolName.Length - 1) : instrument.SymbolName,
                        FullName = instrument.CompanyName,
                        NscCode = instrument.Code,//
                        Bourse = instrument.BourseId.Value,//
                        InstrumentId = instrument.InstrumentId,//
                        Tick = instrument.Tick,
                        BuyCommissionRate = instrument.BuyCommisionRate,
                        SellCommissionRate = instrument.SellCommisionRate,

                    });
                    var omsId = JsonConvert.SerializeObject(instrument.Id);
                    var omsIdContent = Encoding.UTF8.GetBytes(omsId);
                    var content = Encoding.UTF8.GetBytes(serializedData);
                    _cache.Set("Instrument" + instrument.Code, content, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(1) });

                    content = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Id = instrument.Id }));
                    _cache.Set("omsId" + instrument.InstrumentId, omsIdContent, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(1) });

                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        #endregion


        #region instrulentTopics

        public List<string> GetHubsByCustomer(UserHub model)
        {
            var allHubs = _cache.Get("KafkaUserId_"+model.CustomerId);
            List<string> res = new();
            //foreach (var hub in allHubs)
           // {
                //TODO
                //var data = _cache.Get(hub);
                var h = JsonConvert.DeserializeObject<UserHubResult>(Encoding.Default.GetString(allHubs));
                res.AddRange(h.Hubs);
                //if (h.HubId == model.HubId)
                //    _cache.Remove(hub);

           // }
            // var content = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
            //_cache.Set("pushNotificationByInstrument" + model.NscCode, content, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(1) });

            return res;
        }

        public void CustomerSelectInstrument(SelectInstrumentInput model)
        {
           // var allHubs = _redis.Keys(pattern: "pushNotificationByInstrument*" );
            var allHubs = _redis.Keys(pattern: "pushNotificationByInstrument" +model.NscCode);
            CustomerSelectInstrumentModel h = new();
            
            if (allHubs != null)
            {
                foreach (var hub in allHubs)
                {
                    //TODO
                     var data = _cache.Get(hub);
                    h = JsonConvert.DeserializeObject<CustomerSelectInstrumentModel>(Encoding.Default.GetString(data));
                    // model.Persons = (List<string>)model.Persons.Distinct().ToList();

                }
                if (h.Persons != null)
                {
                    h.Persons.Add(model.Person);
                }
                else
                {
                    h.NscCode = model.NscCode;
                    h.Persons = new List<Persons> { model.Person };
                }
            }
            
                
            
            var content = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(h));
            _cache.Set("pushNotificationByInstrument" + model.NscCode, content, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(1) });
           

        }

        public async Task<List<string>> GetInstrumentHubs(string NscCode)
        {

            var allHubs = _redis.Keys(pattern: "pushNotificationByInstrument" + NscCode);
            List<Persons> result = new ();
            foreach (var hub in allHubs)
            {
                var data =  await _cache.GetAsync(hub);
                var ins = JsonConvert.DeserializeObject<CustomerSelectInstrumentModel>(Encoding.Default.GetString(data));
                result.AddRange(ins.Persons.ToList());
                
            }


            //result = result.Distinct().ToList();
            var a = (List<string>)result.Select(x => x.Hubs);
            return a;
        }

        #endregion
    }
}
