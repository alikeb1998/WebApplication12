
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Mvc;

namespace Izi.Online.ViewModels.Instruments
{
    public class NewWatchList:ViewBaseModel
    {
        public NewWatchList()
        {
            InstrumentsId = new List<long>();
        }
   
        [Required(ErrorMessage = "اجباری")]
        public string WatchListName { get; set; }
        
        public List<long> InstrumentsId { get; set; }
    }

   
}
