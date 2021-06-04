using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ShopProject.Constants;
using ShopProject.Entities;
using ShopProject.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject
{
    public static class MigrationConfig
    {
        public static void SeedData(UserManager<AppUser> userManager,
                  RoleManager<AppRole> roleManager)
        {
            var adminRoleName = "Admin";
            var userRoleName = "User";

            var roleResult = roleManager.FindByNameAsync(adminRoleName).Result;
            if (roleResult == null)
            {
                var roleresult = roleManager.CreateAsync(new AppRole
                {
                    Name = userRoleName                    

                }).Result;
            }
            roleResult = roleManager.FindByNameAsync(userRoleName).Result;
            if (roleResult == null)
            {
                var roleresult = roleManager.CreateAsync(new AppRole
                {
                    Name = userRoleName

                }).Result;
            }


            var email = "admin@gmail.com";
            var findUser = userManager.FindByEmailAsync(email).Result;
            if (findUser == null)
            {
                var user = new AppUser
                {
                    Email = email,
                    UserName = email,
                   // Image = "https://cdn.pixabay.com/photo/2017/07/28/23/34/fantasy-picture-2550222_960_720.jpg",
                   // Age = 30,
                    PhoneNumber = "+380957476156",
                    Description = "PHP programmer"
                };
                var result = userManager.CreateAsync(user, "Qwerty1").Result;

                result = userManager.AddToRoleAsync(user, adminRoleName).Result;
            }

            email = "user@gmail.com";
            findUser = userManager.FindByEmailAsync(email).Result;
            if (findUser == null)
            {
                var user = new AppUser
                {
                    Email = email,
                    UserName = email,
                    //Image = "https://cdn.pixabay.com/photo/2017/07/28/23/34/fantasy-picture-2550222_960_720.jpg",
                    //Age = 30,
                    PhoneNumber = "+380988005535",
                    Description = "User"
                };
                var result = userManager.CreateAsync(user, "Qwerty2").Result;

                result = userManager.AddToRoleAsync(user, userRoleName).Result;
            }
        }

        public static void ApplyMigrations( IServiceProvider service)
        {
            using (var serviceScope = service.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
               
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                SeedData(userManager, roleManager);





                //var context = serviceScope.ServiceProvider.GetRequiredService<EFContext>();

                //if (!roleManager.Roles.Any())
                //{
                //    var role = new AppRole
                //    {
                //        Name = Roles.Admin
                //    };
                //    var result = roleManager.CreateAsync(role).Result;
                //}                
                   

            }
        }
    }
}
