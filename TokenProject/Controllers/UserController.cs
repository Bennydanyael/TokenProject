using DataProvider.DataContext;
using Domain.Models;
using JWTManager.Abstract;
using JWTManager.Handler;
using JWTManager.Handler.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

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
                if (_isExist != null) _response = _tokenService.GenerateToken(_request);
                else return NotFound();
            }
            return Ok(_response);
        }

        [Authorize, HttpGet]
        public async Task<ActionResult<List<UserAccount>>> GetUsers() => await _context.UserAccounts.ToListAsync();

        [AllowAnonymous, HttpPost]
        public async Task<ActionResult<AuthenticationRequest>> ForgotPassword([FromBody]AuthenticationRequest _request, CancellationToken _cancellationToken)
        {
            var _isExist = await _context.UserAccounts.FirstOrDefaultAsync(p => p.Username == _request.Username).ConfigureAwait(false);
            if (_isExist != null)
            {
                _isExist.Password = _request.Password;
                await _context.SaveChangesAsync();
            }
            else return NotFound();
            return Ok("SUCCESS");
        }

        //[AllowAnonymous, HttpPost]
        //public async Task<ActionResult> Registration([FromBody]AuthenticationRequest _request)
        //{
        //    if (_request == null) return NoContent();
        //    try
        //    {
        //        UserAccount _user = new UserAccount();
        //        _user.Username = _request.Username;
        //        _user.Password = _request.Password;
        //        _user.Role = _request.Role;
        //        await _context.AddAsync(_user);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception _ex)
        //    {
        //        return BadRequest("Error : " + _ex.Message);
        //    }
        //    return Ok("SUCCESS");
        //}
    }
}
