
using WangZhen.Techniques.Auth.Api.Models;
using WangZhen.Techniques.Auth.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WangZhen.Techniques.Auth.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody]AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message= "Username or password is incorrect" });

            return Ok(response);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}",Name =nameof(GetById))]
        public IActionResult GetById(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            if (currentUserId != id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var user = _userService.GetById(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post(RegisterRequest register)
        {
            var result = _userService.Register(register);
            if (result == null)
                return BadRequest();


            return CreatedAtRoute(nameof(GetById), new { id = result.Id }, result);


        }

    }
}
