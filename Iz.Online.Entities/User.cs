﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Entities
{
    public class Customer
    {
        public string Id { get; set; }
        public string Token  { get; set; }
        public  ICollection<CustomerHubs> CustomersHubs { get; set; }
        public ICollection<WatchList> WathLists { get; set; }

    }
}
