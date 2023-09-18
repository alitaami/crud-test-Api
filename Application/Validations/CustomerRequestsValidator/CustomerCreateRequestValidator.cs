using Application.Features.Properties.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.CustomerRequestsValidator
{
    public class CustomerCreateRequestValidator : AbstractValidator<CreateCustomerRequest>  
    {
        public CustomerCreateRequestValidator()
        {
            RuleFor(x => x.CustomerRequest)
                .SetValidator(new Validations.CustomerViewModelValidator.CustomerValidator());
      
        }
        
    }
}
