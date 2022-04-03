using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Izi.Online.ViewModels.ShareModels
{
    public class Respond<T>
    {
        public ResultModel<T> Model { get; set; }
        public Respond(ResultModel<T> Model)
        {
            this.Model = Model;
        }
        //public IActionResult ActionRespond(ResultModel<T> Model)
        //{
        //    var res = 1;
        //    return Ok();
        //}

    }
}
