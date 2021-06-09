using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopProject.Constants;
using ShopProject.Entities;
using ShopProject.Entities.Identity;
using ShopProject.Services;
using ShopProject.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopProject.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IJwtTokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public string _url = "http://localhost:5000/img/";

        public AccountController(IJwtTokenService tokenService, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("show")]
        //[Authorize(Roles = Roles.Admin)]
        public IActionResult action()
        {
            var list = new List<Products>()
            {
                new Products
                {
                    Name="Олівці",
                    Description = "Різнокольорові олівці відомої фірми Марко (Чехія). Набір 24 кольори. Яскраві, приємні для сприйняття",
                    Price=35,
                    Image=_url+"01.jpg"
                },
                new Products
                {
                    Name="Набір",
                    Description = "Набір канцелярського приладдя для школярів. Включає кольорові олівці, фломастери, фарби акварельні, гуаші, лінійки, клей",
                    Price=305,
                    Image=_url+"02.jpg"
                },
                new Products {
                    Name="Офісне приладдя",
                    Description = "Набір канцтоварів для офісу. До складу входять: олівці, ручки, підставка, блокноти, калькулятор, лінійка",
                    Price=520,
                    Image=_url+"03.jpg"
                   },
                new Products {
                    Name="Фломастери",
                    Description = "Набір різнокольорових фломастерів чеської фірми Кох-і-нор. 36 фломастерів відмінної якості з екологічними барвниками",
                    Price=75,
                    Image=_url+"04.jpg"
                    },
                new Products {
                    Name="Палички",
                    Description = "Палички для лічби. Призначені для учнів дошкільного та молодшого шкільного віку. 40 штук",
                    Price=16,
                    Image=_url+"05.jpg"
                    }

            };
            return Ok(list);
        }

        [HttpPost]
       // [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Post([FromBody]UserModel userModel)
        {
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            if (user == null)
                return BadRequest("Not Found");

            var result = await _signInManager.PasswordSignInAsync(user, userModel.Password, false, false);
            if (!result.Succeeded)
                return BadRequest("Not Found");

            return Ok(
                new
                {
                    token = _tokenService.Authentificate(user)
                });             

        }

    }
}
