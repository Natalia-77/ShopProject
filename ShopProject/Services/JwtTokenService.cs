using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopProject.Entities.Identity;
using ShopProject.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopProject.Services
{
   
    public interface IJwtTokenService
    {
        public string Authentificate(AppUser user);
    }

    public class JwtTokenService : IJwtTokenService
    {
       
        private readonly AppSettings _appSettings;
        private readonly UserManager<AppUser> _userManager;
        public JwtTokenService(IOptions<AppSettings>appsettings, UserManager<AppUser> userManager)
        {
            _appSettings = appsettings.Value;
            _userManager = userManager;
        }
       
        public string Authentificate(AppUser appUser)
        {
            
            var roles = _userManager.GetRolesAsync(appUser).Result;
            var roleClaims = new List<Claim>()
            {
                 new Claim("id",appUser.Id.ToString()),
                 new Claim("name",appUser.UserName)
            };

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
           
            var key = Encoding.ASCII.GetBytes(_appSettings.Key);
            var signKey = new SymmetricSecurityKey(key);
            var singCredentials = new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
               signingCredentials: singCredentials,
               expires: DateTime.Now.AddDays(100),
               claims: roleClaims
               );
            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }

        






        //private readonly EFContext _context;
        //private readonly UserManager<AppUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly JWT _jwt;

        //public JwtTokenService(UserManager<AppUser> userManager, EFContext context,
        //   RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt )
        //{
        //    _context = context;
        //    _userManager = userManager;
        //    _roleManager = roleManager;
        //    _jwt = jwt.Value;
        //}

        //public async Task<string> RegisterAsync(RegisterModel model)
        //{
        //    var user = new AppUser
        //    {
        //        UserName = model.Username,
        //        Email = model.Email,
        //        FirstName = model.FirstName,
        //        LastName = model.LastName
        //    };
        //    var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
        //    if (userWithSameEmail == null)
        //    {
        //        var result = await _userManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            await _userManager.AddToRoleAsync(user,Authorizations.default_role.ToString());

        //        }
        //        return $"User Registered with username {user.UserName}";
        //    }
        //    else
        //    {
        //        return $"Email {user.Email } is already registered.";
        //    }
        //}

        //public async Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model)
        //{
        //    var authenticationModel = new AuthenticationModel();
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        authenticationModel.IsAuthenticated = false;
        //        authenticationModel.Message = $"No Accounts Registered with {model.Email}.";
        //        return authenticationModel;
        //    }
        //    if (await _userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        authenticationModel.IsAuthenticated = true;
        //        JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
        //        authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        //        authenticationModel.Email = user.Email;
        //        authenticationModel.UserName = user.UserName;
        //        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        //        authenticationModel.Roles = rolesList.ToList();


        //        //if (user.RefreshTokens.Any(a => a.IsActive))
        //        //{
        //        //    var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
        //        //    authenticationModel.RefreshToken = activeRefreshToken.Token;
        //        //    authenticationModel.RefreshTokenExpiration = activeRefreshToken.Expires;
        //        //}
        //        //else
        //        //{
        //        //    var refreshToken = CreateRefreshToken();
        //        //    authenticationModel.RefreshToken = refreshToken.Token;
        //        //    authenticationModel.RefreshTokenExpiration = refreshToken.Expires;
        //        //    user.RefreshTokens.Add(refreshToken);
        //        //    _context.Update(user);
        //        //    _context.SaveChanges();
        //        //}

        //        return authenticationModel;
        //    }
        //    authenticationModel.IsAuthenticated = false;
        //    authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
        //    return authenticationModel;
        //}
        //private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
        //{
        //    var userClaims = await _userManager.GetClaimsAsync(user);
        //    var roles = await _userManager.GetRolesAsync(user);

        //    var roleClaims = new List<Claim>();

        //    for (int i = 0; i < roles.Count; i++)
        //    {
        //        roleClaims.Add(new Claim("roles", roles[i]));
        //    }

        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(JwtRegisteredClaimNames.Email, user.Email)
        //       // new Claim("uid", user.Id)
        //    }
        //    .Union(userClaims)
        //    .Union(roleClaims);

        //    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        //    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        //    var jwtSecurityToken = new JwtSecurityToken(
        //        issuer: _jwt.Issuer,
        //        audience: _jwt.Audience,
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
        //        signingCredentials: signingCredentials);
        //    return jwtSecurityToken;
        //}

        //public async Task<string> AddRoleAsync(AddRoleModel model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        return $"No Accounts Registered with {model.Email}.";
        //    }
        //    if (await _userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        var roleExists = Enum.GetNames(typeof(Authorizations.Roles)).Any(x => x.ToLower() == model.Role.ToLower());
        //        if (roleExists)
        //        {
        //            var validRole = Enum.GetValues(typeof(Authorizations.Roles)).Cast<Authorizations.Roles>().Where(x => x.ToString().ToLower() == model.Role.ToLower()).FirstOrDefault();
        //            await _userManager.AddToRoleAsync(user, validRole.ToString());
        //            return $"Added {model.Role} to user {model.Email}.";
        //        }
        //        return $"Role {model.Role} not found.";
        //    }
        //    return $"Incorrect Credentials for user {user.Email}.";

        //}

        //public string CreateToken(AppUser user)
        //{
        //    var roles = _userManager.GetRolesAsync(user).Result;
        //    roles = roles.OrderBy(x => x).ToList();
        //    var query = _context.Users.AsQueryable();
        //    //var image = user.Image;

        //    //if (image == null)
        //    //{
        //    //    image = _configuration.GetValue<string>("DefaultImage");
        //    //}


        //    List<Claim> claims = new List<Claim>()
        //    {
        //        new Claim("id",user.Id.ToString()),
        //        new Claim("name",user.UserName),

        //    };
        //    foreach (var role in roles)
        //    {
        //        claims.Add(new Claim("roles", role));
        //    }

        //    //var now = DateTime.UtcNow;
        //    var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Cats"));
        //    var signinCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

        //    var jwt = new JwtSecurityToken(
        //        signingCredentials: signinCredentials,
        //        expires: DateTime.Now.AddDays(1),
        //        claims: claims
        //        );
        //    return new JwtSecurityTokenHandler().WriteToken(jwt);
        //}

    }
}
