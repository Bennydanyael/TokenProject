using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTManager.Handler.ViewModels
{
    public class AuthenticationRespone
    {
        public string Username { get; set; }
        public int ExpiredIn { get; set; }
        public string JwtToken { get; set; }
        public string Role { get; set; }
    }
}
