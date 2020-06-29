using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationModel model)
        {
            var user = _userRepo.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or Password is incorrect." });
            }
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Regeister([FromBody] AuthenticationModel model)
        {
            bool ifUserNameUniue = _userRepo.IsUniqueUser(model.Username);
            if(!ifUserNameUniue)
            {
                return BadRequest(new { message = "Username already exists..." });
            }
            var user = _userRepo.Register(model.Username, model.Password);

            if(user == null)
            {
                return BadRequest(new { message = "Error attempting to Register..." });
            }

            // User Opject could be returned...
            return Ok();
        }
    }
}
