using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopProject.Services;
using ShopProject.ViewModels;


namespace ShopProject.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IJwtTokenService _tokenService;
        public AccountController(IJwtTokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public IActionResult Post([FromBody]UserModel userModel)
        {
            var user_item = _tokenService.Authentificate(userModel.UserName, userModel.Password);

            if (user_item == null)
                return BadRequest(new {message="Not found user!" });

            return Ok(user_item);

        }

    }
}
