using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.Instruments
{
    public class ValidatedWatchList
    {
        public bool IsValidated { get; set; }
        public ResultModel<WatchListDetails> Model{get;set;}
}
}
