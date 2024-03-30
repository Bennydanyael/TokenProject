using DataProvider.DataContext;
using JWTManager.Abstract;
using JWTManager.Handler;
using JWTManager.Handler.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace TokenProject.Controllers
{
    //[Authorize]
    [ApiController, Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IJWTTokenService _tokenService;
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context, IJWTTokenService tokenService
            ) 
        {
            _context = context;
            _tokenService = tokenService;
        }

        [AllowAnonymous, HttpPost]
        public async Task<ActionResult> Login([FromBody]AuthenticationRequest _request, CancellationToken _cancellationToken)
        {
            AuthenticationRespone _response = new AuthenticationRespone();
            if (_request == null) return BadRequest("Invalid Credentials..");
            else
            {
                var _password = string.Empty;
                var _isExist = await _context.UserAccounts.FirstOrDefaultAsync(c => c.Username == _request.Username && c.Password == _request.Password).ConfigureAwait(false);
                //if (_request.Username == "Admin" && _request.Password == "Admin@12345")
                if (_isExist != null)
                {
                    //_password = _isExist.Password;
                    _response = _tokenService.GenerateToken(_request);
                }
                else return NotFound();
            }
            return Ok(_response);
        }
    }
}
