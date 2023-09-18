using Application.Features.Behavior.Contracts;
using Application.Models;
using Application.Models.ViewModels;
using Application.Repository;
using Application.Services.Interfaces;
using AutoMapper;
using Common.Resources;
using Common.Utilities;
using Domain;
using Domain.ApiResult;
using Domain.Entities;
using EstateAgentApi.Services.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Properties.Commands
{
    public class CreateCustomerRequest : IRequest<ServiceResult>, /* only classes those inherits this can have validation */ IValidatable
    {
        public CustomerViewModel CustomerRequest { get; set; }

        public CreateCustomerRequest(CustomerViewModel newProperty)
        {
            CustomerRequest = newProperty;
        }
    }
    public class CreatePropertyRequestHandler : ServiceBase<CreatePropertyRequestHandler>, IRequestHandler<CreateCustomerRequest, ServiceResult>
    {
        private readonly ICustomerService _customer;
        public CreatePropertyRequestHandler(ILogger<CreatePropertyRequestHandler> logger,ICustomerService customer):base(logger)  
        {
            _customer = customer;
        }

        public async Task<ServiceResult> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _customer.AddCustomer(request.CustomerRequest, cancellationToken);
                 
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }
    }
}
