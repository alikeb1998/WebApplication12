using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.DataAccess;
using Iz.Online.Entities;
using Iz.Online.Files;
using Iz.Online.Reopsitory.IRepository;
using Microsoft.Extensions.Configuration;

namespace Iz.Online.Reopsitory.Repository
{
    public class BaseRepository : IBaseRepository
    {
        protected readonly OnlineBackendDbContext _db;

        public BaseRepository(OnlineBackendDbContext dataBase )
        {
            _db = dataBase;
        }

        public void LogException(Exception exception)
        {
            var ex = new ExceptionsLog()
            {
                Id = Guid.NewGuid().ToString(),
                ExceptionTime = DateTime.Now,
                Message = "_" + ExceptionHandler.ExceptionToString(exception),
                StackTrace = "_" + exception.StackTrace

            };
            _db.Exceptions.Add(ex);
            _db.SaveChanges();
        }

     

        public string GetOmsToken(string token)
        {
            var custmer = _db.Customer.FirstOrDefault(x => x.LocalToken.Contains( token));

            if (custmer == null)
                return null;

            return custmer.OmsToken;
        }

        public bool LocalTokenIsValid(string token)
        {
            return _db.Customer.Any(x => x.LocalToken == token && x.TokenExpireDate > DateTime.Now);
            
        }
    }
}
