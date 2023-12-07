using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactAPI.Models
{
    [Table("tblProduct")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public float SalePrice { get; set; }
        public float Sold { get; set; }
        public string Address { get; set; }
        public Category Category { get; set; }
    }
}