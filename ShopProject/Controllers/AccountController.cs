using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopProject.Constants;
using ShopProject.Entities;
using ShopProject.Entities.Identity;
using ShopProject.Services;
using ShopProject.UIHelper;
using ShopProject.ViewModels;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
        private EFContext _context;
        public string _url = "http://localhost:5000/img/";

        public AccountController(IJwtTokenService tokenService, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, EFContext context)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        [Route("show")]
        //тут не повинно бути ролі,бо тут вивід товарів для всіх має бути!!!
        //[Authorize(Roles = Roles.Admin)]
        public IActionResult action()
        {
            var list = _context.Products.Select(
               x => new
               {
                   x.Id,
                   x.Name,
                   x.Price,                  
                   x.Image,
                   x.Description
               });


            //var list = new List<Products>()
            //{
            //    new Products
            //    {
            //        Name="Олівці",
            //        Description = "Різнокольорові олівці відомої фірми Марко (Чехія). Набір 24 кольори. Яскраві, приємні для сприйняття",
            //        Price=35,
            //        Image=_url+"01.jpg"
            //    },
            //    new Products
            //    {
            //        Name="Набір",
            //        Description = "Набір канцелярського приладдя для школярів. Включає кольорові олівці, фломастери, фарби акварельні, гуаші, лінійки, клей",
            //        Price=305,
            //        Image=_url+"02.jpg"
            //    },
            //    new Products {
            //        Name="Офісне приладдя",
            //        Description = "Набір канцтоварів для офісу. До складу входять: олівці, ручки, підставка, блокноти, калькулятор, лінійка",
            //        Price=520,
            //        Image=_url+"03.jpg"
            //       },
            //    new Products {
            //        Name="Фломастери",
            //        Description = "Набір різнокольорових фломастерів чеської фірми Кох-і-нор. 36 фломастерів відмінної якості з екологічними барвниками",
            //        Price=75,
            //        Image=_url+"04.jpg"
            //        },
            //    new Products {
            //        Name="Палички",
            //        Description = "Палички для лічби. Призначені для учнів дошкільного та молодшого шкільного віку. 40 штук",
            //        Price=16,
            //        Image=_url+"05.jpg"
            //        }

            //};
            return Ok(list);
        }

        [HttpPost]
        [Route("add")]
        //[Authorize(Roles = Roles.Admin)]
        public IActionResult AddProduct([FromBody] ProductModel model)
        {
            _context.Products.Add(new Products
            {
                Name = model.Name,
                Description=model.Description,
                Price=model.Price,              
                Image = model.Image
            });

            var dir = Directory.GetCurrentDirectory();
            var ext = Path.GetExtension(model.Image);
            var dirSave = Path.Combine(dir, "Photos");
            var imageName = Path.GetRandomFileName() + ext;
            var imageSaveFolder = Path.Combine(dirSave, imageName);
            var imagen = model.Image.LoadBase64();
            imagen.Save(imageSaveFolder, ImageFormat.Jpeg);

            _context.SaveChanges();
            return Ok();
        }



        [HttpPost]
        //тут теж,бо на даному етапі ми просто шукаємо користувача.
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
