using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.News
{
    public class ObserverMessage
    {
        public Guid Id { get; set; }    
        public string Title { get; set; }
        public DateTime Date { get; set; }  
        public string Text { get;set; } 
    }
}
