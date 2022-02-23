using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Izi.Online.ViewModels.ShareModels;

namespace Izi.Online.ViewModels.Instruments
{
    public class AddCommentForInstrument : ViewBaseModel
    {
        public  int Id { get; set; }
        public string Comment { get; set; }
    }
}
