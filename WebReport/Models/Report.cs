using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebReport.Models
{

    public class ReportClass
    {
        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime OrdersDate { get; set; }

        [Display(Name = "от 0 до 1000")]
        public int LowAmountCount { get; set; }

        [Display(Name = "от 1001 до 5000")]
        public int AverageAmountCount { get; set; }

        [Display(Name = "от 5001")]
        public int HighAmountCount { get; set; }
    }
}
