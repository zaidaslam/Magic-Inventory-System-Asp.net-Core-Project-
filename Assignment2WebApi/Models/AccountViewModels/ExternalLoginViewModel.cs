using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2WebApi.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "StoreID")]
        [Range(1, 5, ErrorMessage = "Please enter valid StoreID")]
        public int? StoreID { get; set; }
    }
}
