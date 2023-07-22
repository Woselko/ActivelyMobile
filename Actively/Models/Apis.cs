using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actively.Models
{
    internal class Apis
    {
        public const string AuthenticateUser = "/Authentication/Login";
        public const string RegisterUser = "/Authentication/Register";
        public const string RefreshToken = "/Authentication/RefreshToken";
    }
}
