﻿using Iz.Online.OmsModels.ResponsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.IO
{
    public class ActiveOrders:OmsResponseBaseModel
    {
    public List<Iz.Online.OmsModels.ResponsModels.IO.ActiveOrder> ActiveIoOrders { get; set; }
    }
}
