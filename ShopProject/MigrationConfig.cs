
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
        
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
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
            if (!context.Products.Any())
            {
                var products = new List<Products>()
                {
                        new Products
                        {
                             Name="Олівці",
                             Description = "Різнокольорові олівці відомої фірми Марко (Чехія). Набір 24 кольори. Яскраві, приємні для сприйняття",
                             Price=35,
                             //Image="01.jpg"
                            //Image="https://nat77.ga/img/9.jpg" ,

                        },
                        new Products
                        {
                            Name="Набір",
                            Description = "Набір канцелярського приладдя для школярів. Включає кольорові олівці, фломастери, фарби акварельні, гуаші, лінійки, клей",
                            Price=305,
                            //Image="02.jpg"
                        },
                        new Products {
                            Name="Офісне приладдя",
                            Description = "Набір канцтоварів для офісу. До складу входять: олівці, ручки, підставка, блокноти, калькулятор, лінійка",
                            Price=520,
                           // Image="03.jpg"
                        },
                        new Products {
                            Name="Фломастери",
                            Description = "Набір різнокольорових фломастерів чеської фірми Кох-і-нор. 36 фломастерів відмінної якості з екологічними барвниками",
                            Price=75,
                           // Image="04.jpg"
                        },
                        new Products {
                            Name="Палички",
                            Description = "Палички для лічби. Призначені для учнів дошкільного та молодшого шкільного віку. 40 штук",
                            Price=16,
                           // Image="05.jpg"
                        }

                };
                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }

    }
}
