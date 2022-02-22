using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Users
{
    public class UsersHubIds
    {
        public UsersHubIds()
        {
            HubId = new List<string>();
        }
        public string  CustomerId { get; set; }
        public List<string> HubId { get; set; }
    }
}
