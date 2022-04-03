using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Izi.Online.ViewModels.ShareModels
{
    public  class Respond<T>:ControllerBase
    {
        public ResultModel<T> Model { get; set; }
        public IActionResult ActionRespond(ResultModel<T> Model)
        {
            this.Model = Model; 
            return Model.StatusCode switch
            {
                200 => Ok(Model),
                401 => Unauthorized(Model),
                404 => NotFound(Model),
                _ => BadRequest(Model),
            };
        }

    }
}
