using Application.Features.Behavior.Contracts;
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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Properties.Commands
{
    public class DeleteCustomerRequest : IRequest<ServiceResult>
    {
        public int PropertyId { get; set; }

        public DeleteCustomerRequest(int propertyId)
        {
            PropertyId = propertyId;
        }
    }

    public class DeletePropertyRequestHandler : ServiceBase<DeletePropertyRequestHandler>, IRequestHandler<DeleteCustomerRequest, ServiceResult>
    {
        private readonly ICustomerService _customer;
        public DeletePropertyRequestHandler(ILogger<DeletePropertyRequestHandler> logger, ICustomerService customer)
       : base(logger)
        {
            _customer = customer;
        }

        public async Task<ServiceResult> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _customer.DeleteCustomerById(request.PropertyId, cancellationToken);
 
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
