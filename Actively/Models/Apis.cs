using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actively.Models
{
    public class Apis
    {
        public const string AuthenticateUser = "/Authentication/Login";
        public const string RegisterUser = "/Authentication/Register";
        public const string RefreshToken = "/Authentication/RefreshToken";


        public const string GetSupportedCultures = "/Language/GetSupportedCultures";
        public const string ChangeLanguage = "/Language/ChangeLanguageApi";
    }
}
