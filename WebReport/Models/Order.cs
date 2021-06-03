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


        [Display(Name = "Сумма")]
        [Required]
        public decimal? Amount { get; set; }


        [Display(Name = "Дата")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
    }
}
