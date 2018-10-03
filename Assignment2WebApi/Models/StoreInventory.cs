using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2WebApi.Models
{
    public class StoreInventory
    {
        
        public int StoreID { get; set; }
        public Store Store { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int StockLevel { get; set; }
    }
}
