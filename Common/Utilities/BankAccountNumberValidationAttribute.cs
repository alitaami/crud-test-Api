using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Common.Utilities
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class BankAccountNumberValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return true; // Allow null or empty values (modify this behavior if needed).
            }

            string bankAccountNumber = value.ToString();

            // Add your bank account number validation logic here.
            // You can use regular expressions or other methods to validate bank account numbers.

            // For example, a simple regex validation:
            // Replace this with your specific validation logic.
            bool isValid = Regex.IsMatch(bankAccountNumber, @"^\d{16}$");

            return isValid;
        }
    }

}
