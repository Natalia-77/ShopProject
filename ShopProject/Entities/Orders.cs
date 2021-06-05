
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
        public int UsersId { get; set; }

        public virtual Users Users { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}
