
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ShopProject.Constants;
using ShopProject.Entities;
using ShopProject.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ShopProject
{
    public static class MigrationConfig
    {


        //public static void ApplyMigrations(IServiceProvider service)
        //{
        //    using var serviceScope = service.GetRequiredService<IServiceScopeFactory>().CreateScope();
        //    var context = serviceScope.ServiceProvider.GetRequiredService<EFContext>();

        //    if (context.Roles.Count() == 0)
        //    {
        //        var roles = new Roles
        //        {
        //            Name = "User"
        //        };
        //        context.Roles.AddRange(roles);
        //        context.SaveChanges();

        //        var users = new Users
        //        {
        //            FirstName = "Петро",
        //            LastName = "Харитонов",
        //            UserName = "petro",
        //            Password = "qwerty",
        //            Token = "",
        //            RolesId = roles.Id
        //        };
        //        context.Users.Add(users);
        //        context.SaveChanges();

        //        var orders = new Orders
        //        {
        //            UsersId = users.Id
        //        };
        //        context.Orders.Add(orders);
        //        context.SaveChanges();

        //        var product = new List<Products>
        //        {
        //            new Products{Name="Набір олівців",Price=52.7F,Image="jumbo.jpg",OrdersId=orders.Id},
        //            new Products {Name="Фломастери МАРКО",Price=75.2F,Image="marko.jpg",OrdersId=orders.Id}
        //        };
        //        context.Products.AddRange(product);
        //        context.SaveChanges();               


        //    }
        //}
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var context = serviceScope.ServiceProvider.GetRequiredService<EFContext>();

                if (!roleManager.Roles.Any())
                {
                    var role = new AppRole
                    {
                        Name = Roles.Admin
                    };
                    var result = roleManager.CreateAsync(role).Result;
                }
                if (!userManager.Users.Any())
                {
                    var user = new AppUser
                    {
                        Email = "user@gmail.com",
                        UserName = "user@gmail.com"
                    };
                    var result = userManager.CreateAsync(user, "qwerty").Result;
                   
                    result = userManager.AddToRoleAsync(user, Roles.Admin).Result;
                }
            }
        }

    }
}
