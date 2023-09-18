using Application.Features.Behavior.Contracts;
using Application.Models;
using Application.Models.ViewModels;
using Application.Repository;
using Application.Services.Interfaces;
using Common.Resources;
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
    public class UpdateCustomerRequest : IRequest<ServiceResult>
    {
        public CustomerUpdateViewModel customerUpdate { get; set; }
        public UpdateCustomerRequest(CustomerUpdateViewModel update)
        {
            customerUpdate = update;

        }
    }
    public class UpdatePropertyHandler : ServiceBase<UpdatePropertyHandler>, IRequestHandler<UpdateCustomerRequest, ServiceResult>
    {
        private readonly ICustomerService _customer;
        public UpdatePropertyHandler(ILogger<UpdatePropertyHandler> logger, ICustomerService customer)
           : base(logger)
        {
            _customer = customer;
        }

        public async Task<ServiceResult> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _customer.UpdateCustomer(request.customerUpdate, cancellationToken);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }
    }
}
