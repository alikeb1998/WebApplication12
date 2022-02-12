
using Iz.Online.DataAccess;
using Iz.Online.Entities;
using Iz.Online.Reopsitory.IRepository;

namespace Iz.Online.Reopsitory.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private OnlineBackendDbContext _db;
        public UserRepository(OnlineBackendDbContext dataBase) : base(dataBase)
        {
            _db = dataBase;
        }

        public List<string> GetUserHubs(string userId)
        {

            var hubs = _db.CustomerHubs.Where(x => x.CustomerId == userId).Select(x => x.HubId).ToList();

            return hubs;
        }

        public List<AppConfigs> GetAppConfigs()
        {
           return _db.AppConfigs.ToList();
        }
        public AppConfigs GetAppConfigs(string key)
        {
           return _db.AppConfigs.FirstOrDefault(x=>x.Key==key);
        }

        public void SetToken(TokenStore token)
        {
            var old = _db.Token.FirstOrDefault();
            _db.Token.Remove(old);

            _db.Token.Add(token);
            _db.SaveChanges();
        }
        public string GetToken()
        {
            return _db.Token.FirstOrDefault().Token;
        }
    }
}
