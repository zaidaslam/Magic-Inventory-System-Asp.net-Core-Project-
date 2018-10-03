using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment2WebApi.Models
{
    public class Store
    {
        public int StoreID { get; set; }
        public string Name { get; set; }

        public ICollection<StoreInventory> StoreInventory { get; } = new List<StoreInventory>();
    }
}
