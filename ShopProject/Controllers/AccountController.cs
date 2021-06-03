using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopProject.Entities;
using ShopProject.Entities.Identity;
using ShopProject.Services;
using ShopProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly EFContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtTokenService _IJwtTokenService;

        public AccountController(EFContext context,
           UserManager<AppUser> userManager,
           SignInManager<AppUser> signInManager,
           IJwtTokenService IJwtTokenService)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _IJwtTokenService = IJwtTokenService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //var errrors = CustomValidator.GetErrorsByModel(ModelState);
                return BadRequest("Bad Model");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                return BadRequest(new { invalid = "Даний користувач не знайденний" });
            }

            var result = _signInManager
                .PasswordSignInAsync(user, model.Password, false, false).Result;

            if (!result.Succeeded)
            {
                return BadRequest(new { invalid = "Невірно введений пароль" });
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(
                 new
                 {
                     token = _IJwtTokenService.CreateToken(user)
                 });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var roleName = "User";
            var userReg = _context.Users.FirstOrDefault(x => x.Email == model.Email);
            if (userReg != null)
            {
                return BadRequest(new { invalid = "Цей емейл вже зареєстровано." });
            }

            if (model.Email == null)
            {
                return BadRequest(new { invalid = "Вкажіть пошту." });
            }
            else
            {
                var testmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (!testmail.IsMatch(model.Email))
                {
                    return BadRequest(new { invalid = "Невірно вказана почта." });
                }
            }
            AppUser user = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
               // Age = model.Age,
                Phone = model.Phone,
                Description = model.Description,
            };
            var res = _userManager.CreateAsync(user, model.Password).Result;
            if (!res.Succeeded)
            {
                return BadRequest(new { invalid = " Код доступу має складатись з 8 символів, містити мінімум одну велику літеру! " });
            }
            res = _userManager.AddToRoleAsync(user, roleName).Result;

            if (res.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                var token = _IJwtTokenService.CreateToken(user);
                return Ok(token);
            }
            return BadRequest();
        }
    }
}
