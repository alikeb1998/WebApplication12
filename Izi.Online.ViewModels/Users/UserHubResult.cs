using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Users
{
    public class UserHubResult
    {
        public List<string> Hubs { get; set; }
        public string Token { get; set; }
        public string KafkaId { get; set; }
    }
}
