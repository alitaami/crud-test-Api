using Application.Features.Behavior.Contracts;
using Application.Features.Properties.Commands;
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
    public class GetAllCustomersRequest : IRequest<ServiceResult>
    {
        public GetAllCustomersRequest()
        {

        }
    }
    public class GetAllPropertiesRequestHandler : ServiceBase<GetAllPropertiesRequestHandler>, IRequestHandler<GetAllCustomersRequest,ServiceResult>
    {
        private readonly ICustomerService _customer;
        public GetAllPropertiesRequestHandler(ILogger<GetAllPropertiesRequestHandler> logger, ICustomerService customer): base(logger) 
        {
            _customer = customer;
        }

        public async Task<ServiceResult> Handle(GetAllCustomersRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _customer.GetCustomers();

                if (res is null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);

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
