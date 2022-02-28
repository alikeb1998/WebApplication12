
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

        public void SetUserHub(string UserId, string hubId, string sessionId)
        {
            var serialized = JsonConvert.SerializeObject(new UsersHubIds() { CustomerId = UserId, HubId = hubId,SessionId = sessionId});
            var content = Encoding.UTF8.GetBytes(serialized);
            _cache.Set(hubId, content, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(1) });

        }
        public void DeleteConnectionId(string connectionId)
        {
          //  var redisKeys = _redis.GetServer("localhost", 6379).Keys(pattern: connectionId)
          //.Select(p => p.ToString()).FirstOrDefault();
            _cache.Remove(connectionId);

        }

        public List<UsersHubIds> GetUserHubs(string userId)
        {

            var allKeys = _redis.GetServer("localhost", 6379).Keys().ToList();

            if (allKeys == null)
                return null;

            var result = new List<UsersHubIds>();
            foreach (var key in allKeys)
            {
                var content = _cache.Get(key);
                var serializedModel = System.Text.Encoding.Default.GetString(content);
                var deserializedObject = JsonConvert.DeserializeObject<UsersHubIds>(serializedModel);

                if (deserializedObject.CustomerId == userId)
                    result.Add(deserializedObject);
            }

            return result;

        }
    }
}
