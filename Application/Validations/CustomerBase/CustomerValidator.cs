using Application.Models.ViewModels;
using Common.Utilities;
using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.CustomerViewModelValidator
{
    public class CustomerValidator : AbstractValidator<CustomerViewModel>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.Firstname)
               .NotEmpty().WithMessage("name is required")
               .MaximumLength(10)
               .WithMessage("name should not accept more than 10 chars");

            RuleFor(c => c.Lastname)
               .NotEmpty().WithMessage("Lastname is required")
               .MaximumLength(10)
               .WithMessage("Lastname should not accept more than 10 chars");
          
            RuleFor(c => c.CountryCode)
               .NotEmpty().WithMessage("CountryCode is required")
               .MaximumLength(4)
               .WithMessage("CountryCode should not accept more than 3 chars");

              RuleFor(c => c.PhoneNumber)
               .NotEmpty().WithMessage("PhoneNumber is required")
               .MaximumLength(10)
               .WithMessage("PhoneNumber should not accept more than 11 chars");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Must(IsValidEmail)
                .WithMessage("Invalid email format.");

            RuleFor(np => np.BankAccountNumber)
           .NotEmpty().WithMessage("Bank account number is required.")
           .Must(IsValidBankAccountNumber) // Apply the custom attribute here
           .WithMessage("Invalid bank account number.");

            // Add other validation rules for  other properties as needed.
        }

        private bool IsValidEmail(string email)
        {
            var emailValidator = new EmailValidationAttribute();
            return emailValidator.IsValid(email);
        }
        private bool IsValidBankAccountNumber(string bankAccountNumber)
        {
            var bankAccountValidationAttribute = new BankAccountNumberValidationAttribute();
            return bankAccountValidationAttribute.IsValid(bankAccountNumber);
        }
    }

    public class CustomerUpdateViewModelValidators : AbstractValidator<CustomerUpdateViewModel>
    {
        public CustomerUpdateViewModelValidators()
        {
            RuleFor(c => c.Id)
              .NotNull()
              .WithMessage("id is required"); 

            RuleFor(c => c.Firstname)
               .NotEmpty().WithMessage("name is required")
               .MaximumLength(10)
               .WithMessage("name should not accept more than 10 chars");

            RuleFor(c => c.Lastname)
               .NotEmpty().WithMessage("Lastname is required")
               .MaximumLength(10)
               .WithMessage("Lastname should not accept more than 10 chars");

            RuleFor(c => c.CountryCode)
             .NotEmpty().WithMessage("CountryCode is required")
             .MaximumLength(4)
             .WithMessage("CountryCode should not accept more than 3 chars");

            RuleFor(c => c.PhoneNumber)
             .NotEmpty().WithMessage("PhoneNumber is required")
             .MaximumLength(10)
             .WithMessage("PhoneNumber should not accept more than 11 chars");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Must(IsValidEmail)
                .WithMessage("Invalid email format.");

            RuleFor(np => np.BankAccountNumber)
           .NotEmpty().WithMessage("Bank account number is required.")
           .Must(IsValidBankAccountNumber) // Apply the custom attribute here
           .WithMessage("Invalid bank account number.");

            // Add other validation rules for  other properties as needed.
        }

        private bool IsValidEmail(string email)
        {
            var emailValidator = new EmailValidationAttribute();
            return emailValidator.IsValid(email);
        }
        private bool IsValidBankAccountNumber(string bankAccountNumber)
        {
            var bankAccountValidationAttribute = new BankAccountNumberValidationAttribute();
            return bankAccountValidationAttribute.IsValid(bankAccountNumber);
        }
    }
     
}
