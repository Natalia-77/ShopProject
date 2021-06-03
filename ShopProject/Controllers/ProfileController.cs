using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopProject.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet("")]
        public async Task<IActionResult> Info()
        {
            var userName = User.Claims.FirstOrDefault(x => x.Type == "name").Value;
            var user = await _userManager.FindByNameAsync(userName);
            return Ok(user);
        }

        [HttpGet("users")]
        public async Task<IActionResult> users()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }
    }
}
