using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.Entities.Identity
{
    public class AppUser:IdentityUser <long>
    {
        //[StringLength(255)]
        //public string Image { get; set; }

        //[Range(0, 100, ErrorMessage = "Недопустимый год")]
        //public double Age { get; set; }

        [StringLength(255)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}
