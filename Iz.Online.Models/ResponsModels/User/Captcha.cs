﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.OmsModels.ResponsModels.User
{
    public class Captcha:OmsResponseBaseModel
    {
        public string  Base64 { get; set; }
        public string Id { get; set; }
    }
}
