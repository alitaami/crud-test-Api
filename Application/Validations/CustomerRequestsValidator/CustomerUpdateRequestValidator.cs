using Application.Features.Behavior.Contracts;
using Application.Features.Properties.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.CustomerRequestsValidator
{

    public class CustomerUpdateRequestValidator : AbstractValidator<UpdateCustomerRequest> 
    {
        public CustomerUpdateRequestValidator()
        {
            RuleFor(x => x.customerUpdate)
                .SetValidator(new Validations.CustomerViewModelValidator.CustomerUpdateViewModelValidators());

        }
    }
}
