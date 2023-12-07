using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ReactAPI.Models
{
    [Table("tblLogined")]
    public class Logined
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
    }
}