using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Services.IServices
{
    public interface IUserService
    {
        List<string> UserHubsList(string UserId);
    }
}
