﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Blazor.Models
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string Content { get; set; }
    }
}