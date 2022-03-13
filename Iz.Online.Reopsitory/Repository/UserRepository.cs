
using System.Configuration;
using Iz.Online.DataAccess;
using Iz.Online.Entities;
using Iz.Online.Reopsitory.IRepository;
using Izi.Online.ViewModels.Users;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.Extensions.Configuration;

namespace Iz.Online.Reopsitory.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly OnlineBackendDbContext _db;
        private readonly IDistributedCache _cache;
        private readonly IServer _redis;
        public UserRepository(OnlineBackendDbContext dataBase, IDistributedCache cache, IConnectionMultiplexer redis, IConfiguration configuration) : base(dataBase)
        {
            IConfiguration _configuration = configuration;
            _db = dataBase;
            _cache = cache;

            var redisConnection = _configuration.GetSection("RedisConnection").Get<string>();
            _redis = redis.GetServer(redisConnection);
        }

        public string GetUserLocalToken(string omsId, string omsToken)
        {
            var entity = _db.Customer.FirstOrDefault(x => x.OmsId == omsId);

            if (entity != null)
            {
                if (entity.TokenExpireDate.Date > DateTime.Now.Date)
                {

                }
                else
                {
                    entity.TokenExpireDate = DateTime.Now.Date;
                    _db.SaveChanges();
                }
            }
            else
            {
                entity = new Customer()
                {
                    Id = Guid.NewGuid().ToString(),
                    OmsId = omsId,
                    TokenExpireDate = DateTime.Now.Date,
                    OmsToken = omsToken
                };

                _db.Customer.Add(entity);
                _db.SaveChanges();

            }


            var authClaims = new List<Claim>
                {
                    new Claim("UserId", entity.Id),
                };

            authClaims.Add(new Claim("Roles", "normalCustomer"));

            var token_ = GetToken(authClaims);

            var obj = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token_),
                expiration = token_.ValidTo
            };
            entity.LocalToken = JsonConvert.SerializeObject(obj);
            _db.SaveChanges();
            return entity.LocalToken;
        }

        public void DeleteConnectionId(string connectionId)
        {
            try
            {
                var redisKeys = _redis.Keys().ToList();
                foreach (var key in redisKeys)
                {
                    var oldDataBytes = _cache.Get(key);
                    var oldData = JsonConvert.DeserializeObject<CustomerInfo>(Encoding.Default.GetString(oldDataBytes));
                    if (oldData.Hubs.Contains(connectionId))
                    {
                        oldData.Hubs.Remove(connectionId);
                        var serialized = JsonConvert.SerializeObject(oldData);
                        var content = Encoding.UTF8.GetBytes(serialized);
                        _cache.Set("KafkaUserId_" + oldData.KafkaId, content, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(1) });
                        return;
                    }
                }

            }
            catch (Exception e)
            {

            }
        }

        public bool SetUserInfo(CustomerInfo model)
        {

            try
            {
                var oldData = new CustomerInfo();
                var oldDataBytes = _cache.Get("KafkaUserId_" + model.KafkaId);
                if (oldDataBytes != null)
                {
                    oldData = JsonConvert.DeserializeObject<CustomerInfo>(Encoding.Default.GetString(oldDataBytes));
                }
                else
                {
                    oldData.Hubs = new List<string>();
                    oldData.Token = model.Token;
                    oldData.KafkaId = model.KafkaId;
                }
                oldData.Hubs.Add(model.Hubs.FirstOrDefault());
                oldData.Hubs = oldData.Hubs.Distinct().ToList();
                var serialized = JsonConvert.SerializeObject(oldData);
                var content = Encoding.UTF8.GetBytes(serialized);
                _cache.Set("KafkaUserId_" + model.KafkaId, content, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(1) });
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public CustomerInfo GetUserHubs(string userId)
        {

            try
            {
                var dataBytes = _cache.Get(userId);
                var result = JsonConvert.DeserializeObject<CustomerInfo>(Encoding.Default.GetString(dataBytes));
                return result;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("_configuration[JWT:Secret]"));

            var token = new JwtSecurityToken(
                //issuer: "_configuration[JWT:ValidIssue"],
                //audience:"_configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }



        #region app configs

        public List<Izi.Online.ViewModels.AppConfigs> ConfigData()
        {
            try
            {
                var allInstrument = _redis.Keys(pattern: "AppConfig*");
                List<Izi.Online.ViewModels.AppConfigs> result = new List<Izi.Online.ViewModels.AppConfigs>();
                foreach (var instrument in allInstrument)
                {
                    var data = _cache.Get(instrument);
                    var ins = JsonConvert.DeserializeObject<Izi.Online.ViewModels.AppConfigs>(Encoding.Default.GetString(data));
                    result.Add(ins);
                }
                if (result.Count > 0)
                    return result;

                result = SqlConfigData();
                CacheConfigData();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private bool CacheConfigData()
        {
            try
            {
                var configs = _db.AppConfigs.ToList();

                foreach (var config in configs)
                {
                    var serializedData = JsonConvert.SerializeObject(new Izi.Online.ViewModels.AppConfigs()
                    {
                        Value = config.Value,
                        Description = config.Description
                    });
                    var content = Encoding.UTF8.GetBytes(serializedData);
                    _cache.Set("AppConfig" + config.Key, content, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(1) });
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private List<Izi.Online.ViewModels.AppConfigs> SqlConfigData()
        {
            try
            {
                var configs = _db.AppConfigs.Select(config => new Izi.Online.ViewModels.AppConfigs()
                {
                    Value = config.Value,
                    Description = config.Description
                }).ToList();

                return configs;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Izi.Online.ViewModels.AppConfigs ConfigData(string key)
        {
            var dataBytes = _cache.Get("AppConfig" + key);
            if (dataBytes == null)
                CacheConfigData();

            dataBytes = _cache.Get("AppConfig" + key);


            var result = JsonConvert.DeserializeObject<Izi.Online.ViewModels.AppConfigs>(Encoding.Default.GetString(dataBytes));
            if (result != null)
                return result;

            result = SqlConfigData().FirstOrDefault(x => x.Key == key);
            CacheConfigData();
            return result;
        }

        
        #endregion

    }
}
