using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopProject.Entities.Identity;

namespace ShopProject.Entities
{
        
    public class EFContext : IdentityDbContext<AppUser, AppRole, long, IdentityUserClaim<long>,
                   AppUserRole, IdentityUserLogin<long>,
                   IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public EFContext(DbContextOptions<EFContext> options)
            : base(options)
        {

        }

        public DbSet<Products> Products { get; set; }       

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

           
        }
    }

}
