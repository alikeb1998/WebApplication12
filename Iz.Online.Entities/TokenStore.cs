﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Entities
{
    public class TokenStore
    {
        [Key]
        public string Token { get; set; }
    }
}
