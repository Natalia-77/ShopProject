
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ShopProject.Entities
{
    [Table("tblProducts")]
    public class Products
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required]
        public float Price { get; set; }

        public string Image { get; set; }

        [ForeignKey("Orders")]
        public int OrdersId { get; set; }      

        public virtual Orders Orders { get; set; }

    }
}
