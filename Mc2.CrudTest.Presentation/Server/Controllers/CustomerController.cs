using Domain.ApiResult;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Net;
using WebFramework.ApiBase;
using Application.Features.Properties.Queries;
using Common.Resources;
using Application.Models.Dtos;
using Application.Features.Properties.Commands;
using Application.Models.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Application.Exceptions;

namespace Mc2.CrudTest.Presentation.Server.Controllers
{
    public class CustomerController : APIControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ISender _sender;

        public CustomerController(ILogger<CustomerController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        /// <summary>
        /// Get a customer by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer.</param>
        /// <returns>Returns the customer's information if found, or an appropriate error response.</returns>
        [HttpOptions]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                var res = await _sender.Send(new GetCustomerByIdRequest(id));

                return APIResponse(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }

        /// <summary>
        /// Get a list of customers.
        /// </summary>
        /// <returns>Returns a list of customers if available, or an appropriate error response.</returns>
        [HttpOptions]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CustomerDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var res = await _sender.Send(new GetAllCustomersRequest());

                return APIResponse(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }

        /// <summary>
        /// Add a new customer.
        /// </summary>
        /// <param name="model">The customer information to be added.</param>
        /// <returns>Returns a success response if the customer is added, or an appropriate error response.</returns>
        [HttpOptions]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> AddCustomer(CustomerViewModel model)
        {
            try
            {
                var res = await _sender.Send(new CreateCustomerRequest(model));

                return APIResponse(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                if (ex is CustomValidationException)
                    return InternalServerError(ErrorCodeEnum.InternalError, Resource.ValidationException);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }

        /// <summary>
        /// Update customer information.
        /// </summary>
        /// <param name="model">The updated customer information.</param>
        /// <returns>Returns a success response if the customer is updated, or an appropriate error response.</returns>
        [HttpOptions]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateCustomer(CustomerUpdateViewModel model)
        {
            try
            {
                var res = await _sender.Send(new UpdateCustomerRequest(model));

                return APIResponse(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                if (ex is CustomValidationException)
                    return InternalServerError(ErrorCodeEnum.InternalError, Resource.ValidationException);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }

        /// <summary>
        /// Delete a customer by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to be deleted.</param>
        /// <returns>Returns a success response if the customer is deleted, or an appropriate error response.</returns>
        [HttpOptions]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var res = await _sender.Send(new DeleteCustomerRequest(id));

                return APIResponse(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }
    }
}
