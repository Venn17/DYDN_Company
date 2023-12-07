using ReactAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DYDN_Company.Models
{
    [Table("tblCart")]
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int AccountID { get; set; }
        public Account Account { get; set; }
        public int ProductID { get; set; }
        public ICollection<Product> Products { get; set; }
        public int Quantity { get; set; }
    }
}
