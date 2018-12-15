using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace RamsOnlineShoppingSystem.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(45, ErrorMessage = "The maximum length must be upto 45 characters only")]
        public string Name { get; set; }

       [Range(1, 100)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }



        public string Description { get; set; }

        [Display(Name = "Updated At")]
        [Column(TypeName = "datetime")]
        public DateTime LastUpdated { get; set; }

        public int CategoryId { get; set; }


        public int numberofitems { get; set; }

        public virtual Category Category { get; set; }

           

        public virtual ICollection<OrderedProduct> OrderedProducts { get; set; }
    }
}