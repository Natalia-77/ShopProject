using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public string Email { get; set; }

        // public double Age { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
