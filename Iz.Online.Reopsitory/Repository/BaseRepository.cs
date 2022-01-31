using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.DataAccess;
using Iz.Online.Entities;
using Iz.Online.Files;
using Iz.Online.Reopsitory.IRepository;

namespace Iz.Online.Reopsitory.Repository
{
    public class BaseRepository : IBaseRepository
    {
        protected readonly OnlineBackendDbContext _db;

        public BaseRepository(OnlineBackendDbContext dataBase)
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
    }
}
