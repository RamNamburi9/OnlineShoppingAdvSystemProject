using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RamsOnlineShoppingSystem.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public string CartId { get; set; }

        public int ProductId { get; set; }
        public int Count { get; set; }

        public int shipped { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        public virtual Product Product { get; set; }


    }
}