using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopProject.Constants;
using ShopProject.Entities;
using ShopProject.Entities.Identity;
using ShopProject.Services;
using ShopProject.UIHelper;
using ShopProject.ViewModels;
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
        //public string _url = "http://localhost:5000/img/";
        //public string _url = "http://burokrat.ga/img/";
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

            return Ok(list);
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult AddProduct([FromBody] ProductModel model)
        {

            var shema = Request.Scheme;
            var port = Request.Host.Port;
            var domain = Request.Host.Host;
            if (port != null)
                domain += ":" + port.ToString();
            string _url = $"{shema}://{domain}/img/";

            var dir = Directory.GetCurrentDirectory();
            //var ext = Path.GetExtension(model.Image);
            var dirSave = Path.Combine(dir, "Photos");
            var imageName = Path.GetRandomFileName() + ".jpg";
            var imageSaveFolder = Path.Combine(dirSave, imageName);
            var imagen =model.Image.LoadBase64();
            imagen.Save(imageSaveFolder);


            _context.Products.Add(new Products
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Image = _url +imageName
                //Image=model.Image
            });



            _context.SaveChanges();
            return Ok();
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
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Update(int id, [FromBody] Products prod)
        {
            var res = _context.Products.FirstOrDefault(x => x.Id == id);
            if (prod == null)
            {
                return BadRequest("Error data");
            }

            var dir = Directory.GetCurrentDirectory();            
            var dirSave = Path.Combine(dir, "Photos");
            var imageName = Path.GetRandomFileName() + ".jpg";
            var imageSaveFolder = Path.Combine(dirSave, imageName);
            if (!prod.Image.Contains(".jpg"))
            {
                var imagen = prod.Image.LoadBase64();
                imagen.Save(imageSaveFolder);
                //res.Image = _url + imageName;
                res.Image = prod.Image;
            }
            else
            {
                res.Image = prod.Image;
            }

            res.Name = prod.Name;
            res.Description = prod.Description;
            res.Price = prod.Price;


            if (res == null)
            {
                return NotFound();
            }

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Delete(int id)
        {
            var del_item = _context.Products.Find(id);

            if (del_item == null)
            {
                return NotFound();
            }

            _context.Products.Remove(del_item);
            _context.SaveChanges();

            return NoContent();
        }


    }
}
