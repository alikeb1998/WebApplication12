using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Entities
{
    public class ExceptionsLog
    {
        public string Id { get; set; }
        public DateTime ExceptionTime { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        
    }
}
