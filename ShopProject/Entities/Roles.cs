
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ShopProject.Entities
{
    [Table("tblRoles")]
    public class Roles
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
