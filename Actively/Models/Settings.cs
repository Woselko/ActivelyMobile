﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actively.Models
{
    public class Settings
    {
        public static UserBasicDetail UserBasicDetail { get; set; }
        public static string BaseUrl;/* = "https://localhost:7010";*/
        public static string LanguageCookie { get; set; }

    }
}
