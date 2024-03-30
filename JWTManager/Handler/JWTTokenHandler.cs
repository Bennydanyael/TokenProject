using Domain.Models;
using JWTManager.Abstract;
using JWTManager.Handler.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTManager.Handler
{
    public class JWTTokenHandler : IJWTTokenService
    {
        public const string JWT_SECURITY_KEY = "qwertyuioplkjhgfdsazxcvbnmTralalaTrilili";
        private const int JWT_TOKEN_VALIDATION_MINS = 20;
        private readonly List<UserAccount> _userAccountList;
        private readonly IConfiguration configuration; 
        public JWTTokenHandler(IConfiguration _configuration)
        {
            configuration = _configuration;
            _userAccountList = new List<UserAccount>
            {
                new UserAccount{ Username = "Admin", Password = "Sabeso76", Role = "Administrator" },
                new UserAccount{ Username = "User1", Password = "Sabeso76", Role = "User" }
            };
        }

        public AuthenticationRespone GenerateToken(AuthenticationRequest _request)
        {
            if (string.IsNullOrEmpty(_request.Username) && string.IsNullOrWhiteSpace(_request.Password)) return null;

            var _userAccount = _userAccountList.FirstOrDefault(c => c.Username == _request.Username && c.Password == _request.Password);
            //if (_userAccount == null) return null;
            var _expiredToken = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDATION_MINS);
            //var _key = configuration.GetValue<string>("Jwt:Key");
            //var _tokenKey = Encoding.UTF8.GetBytes(_key);
            var _tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var _claimIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, _request.Username),
                new Claim(ClaimTypes.Role, "Admin")
            });

            var _signCredentials = new SigningCredentials(new SymmetricSecurityKey(_tokenKey), SecurityAlgorithms.HmacSha256Signature);
            var _securityTokeDescriptor = new SecurityTokenDescriptor
            {
                Subject = _claimIdentity,
                Expires = _expiredToken,
                SigningCredentials = _signCredentials,
                Audience = configuration["Jwt:Audience"],
                Issuer = configuration["Jwt:Issuer"]
            };
            var _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var _securityToken = _jwtSecurityTokenHandler.CreateToken(_securityTokeDescriptor);
            var _token = _jwtSecurityTokenHandler.WriteToken(_securityToken);

            return new AuthenticationRespone
            {
                Username = _request.Username,
                ExpiredIn = (int)_expiredToken.Subtract(DateTime.Now).TotalSeconds,
                JwtToken = _token,
                Role = _claimIdentity.RoleClaimType,
            };
        }
    }
}

