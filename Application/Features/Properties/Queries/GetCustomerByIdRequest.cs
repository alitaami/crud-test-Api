using Application.Features.Behavior.Contracts;
using Application.Models;
using Application.Models.Dtos;
using Application.Repository;
using Application.Services.Interfaces;
using AutoMapper;
using Common.Resources;
using Domain;
using Domain.ApiResult;
using EstateAgentApi.Services.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Properties.Queries
{
    public class GetCustomerByIdRequest : IRequest<ServiceResult>
    {
        public int PropertyId { get; set; }

        public GetCustomerByIdRequest(int propertyId)
        {
            PropertyId = propertyId;
        }
    }
    public class GetPropertyByIdRequestHandler : ServiceBase<GetPropertyByIdRequestHandler>, IRequestHandler<GetCustomerByIdRequest, ServiceResult>
    {
        private readonly ICustomerService _customer;
        public GetPropertyByIdRequestHandler(ILogger<GetPropertyByIdRequestHandler> logger, ICustomerService customer) : base(logger)
        {
            _customer = customer;
        }

        public async Task<ServiceResult> Handle(GetCustomerByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _customer.GetCustomerById(request.PropertyId, cancellationToken);
 
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
