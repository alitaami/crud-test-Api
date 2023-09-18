using Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Customer
    {
        public Customer()
        {

        }

        [Key]
        public int Id { get; set; }  // Unique identifier for the customer
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailValidation(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required]
        public string BankAccountNumber { get; set; }
    }
}
