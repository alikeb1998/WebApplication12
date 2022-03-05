
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

namespace Iz.Online.Reopsitory.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly OnlineBackendDbContext _db;
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _redis;
        public UserRepository(OnlineBackendDbContext dataBase, IDistributedCache cache, IConnectionMultiplexer redis) : base(dataBase)
        {
            _db = dataBase;
            _cache = cache;
            _redis = redis;
        }

        public List<AppConfigs> GetAppConfigs()
        {
            return _db.AppConfigs.ToList();
        }

        public AppConfigs GetAppConfigs(string key)
        {
            return _db.AppConfigs.FirstOrDefault(x => x.Key == key);
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
                var redisKeys = _redis.GetServer("localhost", 6379).Keys().ToList();
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
                var oldDataBytes = _cache.Get("KafkaUserId_" + model.KafkaId);
                var oldData = JsonConvert.DeserializeObject<CustomerInfo>(Encoding.Default.GetString(oldDataBytes));
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

                //var allKeys = _redis.GetServer("localhost", 6379).Keys().ToList();
                //_cache.Get(userId);
                //if (allKeys == null)
                //    return null;

                //var result = new List<UsersHubIds>();
                //foreach (var key in allKeys)
                //{
                //    var content = _cache.Get(key);
                //    var serializedModel = System.Text.Encoding.Default.GetString(content);
                //    var deserializedObject = JsonConvert.DeserializeObject<UsersHubIds>(serializedModel);

                //    if (deserializedObject.CustomerId == userId)
                //        result.Add(deserializedObject);
                //}

                //return result;

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

    }
}
