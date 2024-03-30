using JWTManager.Handler.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTManager.Abstract
{
    public interface IJWTTokenService
    {
        AuthenticationRespone GenerateToken(AuthenticationRequest _request);
    }
}
