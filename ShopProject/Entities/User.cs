
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ShopProject.Entities
{
    [Table("tblUsers")]
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(150)]
        public string LastName { get; set; }

        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required, StringLength(120)]
        public string Password { get; set; }

        public string Token { get; set; }

        [ForeignKey("Roles")]
        public int RolesId { get; set; }

        public virtual Roles Roles { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }

    }
}
