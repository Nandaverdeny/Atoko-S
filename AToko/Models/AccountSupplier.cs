using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AToko.Models
{
    public class AccountSupplier
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Password is between 3 and 10")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = " Confirm Password is not match")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
      
    }
}