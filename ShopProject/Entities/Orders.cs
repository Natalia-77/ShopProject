
using ShopProject.Entities.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ShopProject.Entities
{
    [Table("tblOrders")]
    public class Orders
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Users")]
        public long UsersId { get; set; }

        public virtual AppUser Users { get; set; }

        [ForeignKey("Products")]
        public int ProductId { get; set; }

        public virtual Products Products { get; set; }

       

    }
}
