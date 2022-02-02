using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Izi.Online.ViewModels.ShareModels
{
    //[BindProperties]
    public  class ViewBaseModel
    {
        public string Token { get; set; }
        public string CustomerId { get; set; }
    }
}
