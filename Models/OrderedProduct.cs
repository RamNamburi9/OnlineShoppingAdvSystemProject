using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RamsOnlineShoppingSystem.Models
{
    public class OrderedProduct
    {
        [Key]
        [Column(Order = 0)]
        public int ProductId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int CustomerOrderId { get; set; }

        public int Quantity { get; set; }

        public int shipped { get; set; }


        public virtual Product Product { get; set; }
        public virtual CustomerOrder CustomerOrder { get; set; }
    }
}