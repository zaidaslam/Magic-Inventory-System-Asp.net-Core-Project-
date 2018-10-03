using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Assignment2.Models
{
    public class OrderHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        public string CustomerID { get; set; }
    }
}
