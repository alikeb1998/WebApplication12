
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

        public UsersHubIds GetUserHubs(string userId)
        {

            var content = _cache.Get(userId);
            var serializedModel = System.Text.Encoding.Default.GetString(content);
            var deserializedObject = JsonConvert.DeserializeObject<UsersHubIds>(serializedModel);
            return deserializedObject;

            //var hubs = _db.CustomerHubs.Where(x => x.CustomerId == userId).Select(x => x.HubId).ToList();

            //return hubs;
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

        public void SetUserHub(string UserId, string hubId)
        {
            var redisKeys = _redis.GetServer("localhost", 6379).Keys(pattern: UserId)
          .Select(p => p.ToString()).ToList();

            var data = new UsersHubIds { CustomerId = UserId, HubId = new List<string> { hubId } };

            foreach (var redisKey in redisKeys)
            {
                var t = _cache.Get(redisKey);
                var obj = Encoding.Default.GetString(t);
                var des = JsonConvert.DeserializeObject<UsersHubIds>(obj);

                data.HubId.AddRange(des.HubId.ToList());
            }
            var serialized = JsonConvert.SerializeObject(new UsersHubIds() { CustomerId = UserId , HubId= data.HubId.Distinct().ToList()});
            var content = Encoding.UTF8.GetBytes(serialized);
            _cache.Set(UserId, content, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(1) });
        }
    }
}
