using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Izi.Online.ViewModels.ShareModels;

namespace Izi.Online.ViewModels.Instruments
{
    public class EditWatchList: ViewBaseModel
    {
        public EditWatchList()
        {
            InstrumentsId = new List<long>();
        }
        public string Id { get; set; }

        [Required(ErrorMessage = "اجباری")]
        public string WatchListName { get; set; }

        public List<long> InstrumentsId { get; set; }
    }
}
