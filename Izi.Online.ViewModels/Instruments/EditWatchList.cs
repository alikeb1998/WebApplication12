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
            Id = new List<long>();
        }
        public string WatchListId { get; set; }

        [Required(ErrorMessage = "اجباری")]
        public string WatchListName { get; set; }

        public List<long> Id { get; set; }
    }
}
