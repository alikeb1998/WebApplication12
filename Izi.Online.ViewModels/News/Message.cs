using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.News
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get;set; }
        public string Content { get; set; }
    }
}
