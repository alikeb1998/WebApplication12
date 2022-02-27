using Iz.Online.Reopsitory.IRepository;
using Iz.Online.DataAccess;
using Iz.Online.Entities;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Iz.Online.Reopsitory.Repository
{
    public class InstrumentsRepository : BaseRepository, IInstrumentsRepository
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDistributedCache _cache;
        public InstrumentsRepository(OnlineBackendDbContext dataBase, IConnectionMultiplexer redis, IDistributedCache cache) : base(dataBase)
        {
            _redis = redis;
            _cache = cache;
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
                    Tick = x.Tick,
                    BuyCommisionRate = x.BuyCommisionRate,
                    SellCommisionRate = x.SellCommisionRate,

                }).ToList();
                
                return new ResultModel<List<Instruments>>(ins);
            }
            catch (Exception e)
            {
                return new ResultModel<List<Instruments>>(null, false, "خطای پایگاه داده", -1);

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
                return new ResultModel<List<InstrumentBourse>>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public ResultModel<List<InstrumentSector>> GetInstrumentSector()
        {
            try
            {

                return new ResultModel<List<InstrumentSector>>(_db.InstrumentSectors.ToList());
            }

            catch (Exception)
            {
                return new ResultModel<List<InstrumentSector>>(null, false, "خطای پایگاه داده", -1);

            }
        }

        public ResultModel<List<InstrumentSubSector>> GetInstrumentSubSectors()
        {
            try
            {

                return new ResultModel<List<InstrumentSubSector>>(_db.InstrumentSubSectors.ToList());
            }

            catch (Exception)
            {
                return new ResultModel<List<InstrumentSubSector>>(null, false, "خطای پایگاه داده", -1);

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
                return new ResultModel<bool>(false, false, "خطای پایگاه داده", -1);

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
                return new ResultModel<bool>(false, false, "خطای پایگاه داده", -1);

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
                return new ResultModel<bool>(false, false, "خطای پایگاه داده", -1);

            }
        }

        public ResultModel<bool> AddInstrument(Instrument model, int sectorId, int subSectorId, int bourseId, long tick, float BuyCommissionRate, float SellCommissionRate)
        {
            try

            {
                model.SubSectorId = subSectorId;
                model.SectorId = sectorId;
                model.BourseId = bourseId;

                _db.Instruments.Add(model);
                _db.SaveChanges();
                return new ResultModel<bool>(true);

            }
            catch (Exception)
            {
                return new ResultModel<bool>(false, false, "خطای پایگاه داده", -1);

            }
        }
        public ResultModel<bool> UpdateInstruments(Instrument model, int sectorId, int subSectorId, int bourseId, long tick, float BuyCommissionRate, float SellCommissionRate)
        {
            try
            {
                var entity = _db.Instruments.FirstOrDefault(x => x.InstrumentId == model.InstrumentId);


                entity.SubSectorId = subSectorId;
                entity.SectorId = sectorId;
                entity.BourseId = bourseId;
                entity.Tick = tick;
                //////this numbers got from sattar.
                entity.BuyCommisionRate = BuyCommissionRate;
                entity.SellCommisionRate = SellCommissionRate;
                //////
                _db.SaveChanges();
                return new ResultModel<bool>(true);

            }
            catch (Exception e)
            {
                LogException(e);
                return new ResultModel<bool>(false, false, "خطای پایگاه داده", -1);

            }
        }
     
        public ResultModel<long> GetInstrumentId(string nscCode)
        {
            
            var entity = _db.Instruments.Where(x => x.Code == nscCode).Select(x => x.InstrumentId).FirstOrDefault();
            return new ResultModel<long>(entity);
        }

        public ResultModel<bool> AddCommentToInstrument(AddCommentForInstrument model)
        {
            try
            {
                var entity = _db.InstrumentComments.FirstOrDefault(x =>
                    x.CustomerId == model.CustomerId && x.InstrumentId == model.Id);

                if (entity != null)
                {
                    entity.CommentText = model.Comment;
                    _db.SaveChanges();
                    return new ResultModel<bool>(true);
                }
                else
                {
                    _db.InstrumentComments.Add(new InstrumentComment()
                    {
                        InstrumentId = model.Id,
                        CommentText = model.Comment,
                        Id = Guid.NewGuid().ToString(),
                        CustomerId = model.CustomerId
                    });
                    _db.SaveChanges();
                }

                return new ResultModel<bool>(true);
            }
            catch (Exception e)
            {
                return new ResultModel<bool>(false, false, "خطای پایگاه داده", -1);

            }
        }

        public ResultModel<string> GetInstrumentComment(GetInstrumentComment model)
        {
            var entity = _db.InstrumentComments.FirstOrDefault(x =>
                x.CustomerId == model.CustomerId && x.InstrumentId == model.Id);

            if (entity != null)
                return new ResultModel<string>(entity.CommentText);

            return new ResultModel<string>(null, false, "یادداشتی ثبت نشده است", -1);
        }
    }
}
