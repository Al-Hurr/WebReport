using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebReport.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal? Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
    }
}
