using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Reopsitory.IRepository
{
    public interface IBaseRepository
    {
        void LogException(Exception exception);
    }
}
