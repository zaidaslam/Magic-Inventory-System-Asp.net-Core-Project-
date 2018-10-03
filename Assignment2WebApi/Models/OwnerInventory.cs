using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2WebApi.Models
{
    public class OwnerInventory
    {
        [Key, ForeignKey("Product"), Display(Name = "ProductID")]
        public int ProductID { get; set; }
        public Product Product { get; set; }


        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        [Display(Name = "Stock Level")]
        public int StockLevel { get; set; }

    }
}
