using Application.Models.Dtos;
using Application.Models.ViewModels;
using Application.Repository;
using Application.Services.Interfaces;
using AutoMapper;
using Common.Resources;
using Common.Utilities;
using Domain.ApiResult;
using Domain.Entities;
using EstateAgentApi.Services.Base;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CustomerService : ServiceBase<CustomerService>, ICustomerService
    {
        private readonly IRepository<Customer> _repo;
        private readonly IMapper _mapper;

        public CustomerService(ILogger<CustomerService> logger, IRepository<Customer> repo, IMapper mapper) : base(logger)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetCustomerById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _repo.GetByIdAsync(cancellationToken, id);

                if (customer == null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                CustomerDto res = _mapper.Map<CustomerDto>(customer);

                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }
        public async Task<ServiceResult> GetCustomers()
        {
            try
            {
                var customer = _repo.TableNoTracking.ToList();

                if (customer == null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                List<CustomerDto> res = _mapper.Map<List<CustomerDto>>(customer);

                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }
        public async Task<ServiceResult> AddCustomer(CustomerViewModel model, CancellationToken cancellationToken)
        {
            try
            {
                ValidateModel(model);

                Customer customer = _mapper.Map<Customer>(model);

                bool checkPhoneNumber = PhoneNumberValidation.IsValidMobilePhoneNumber(model.PhoneNumber, model.CountryCode);

                if (!checkPhoneNumber)
                    return BadRequest(ErrorCodeEnum.PhoneFormatError, Resource.MobileCheck, null);///

                var check = await CheckConflicts(model.Firstname, model.Lastname, model.DateOfBirth, model.Email, cancellationToken);

                if(!check.Result.Equals(true))
                    return BadRequest(ErrorCodeEnum.BadRequest, Resource.ConflictError, null);///

                var res = _repo.AddAsync(customer, cancellationToken);

                if (res.IsCanceled)
                    return BadRequest(ErrorCodeEnum.BadRequest, Resource.GeneralErrorTryAgain, null);///

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }
        public async Task<ServiceResult> DeleteCustomerById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _repo.GetByIdAsync(cancellationToken, id);

                if (customer == null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                var res = _repo.DeleteAsync(customer, cancellationToken);

                if (res.IsCanceled)
                    return BadRequest(ErrorCodeEnum.BadRequest, Resource.GeneralErrorTryAgain, null);///

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        public async Task<ServiceResult> UpdateCustomer(CustomerUpdateViewModel model, CancellationToken cancellationToken)
        {
            try
            {
                ValidateModel(model);

                bool checkPhoneNumber = PhoneNumberValidation.IsValidMobilePhoneNumber(model.PhoneNumber, model.CountryCode);

                var customer = _repo.GetByIdAsync(cancellationToken, model.Id).Result;

                if (customer == null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                var check = await CheckConflicts(model.Firstname, model.Lastname, model.DateOfBirth,model.Email,cancellationToken);

                if (!check.Result.Equals(true)) 
                        return BadRequest(ErrorCodeEnum.BadRequest, Resource.ConflictError, null);///
                 
                // TODO : check unique fields and conditions  
                customer.BankAccountNumber = model.BankAccountNumber;
                customer.DateOfBirth = model.DateOfBirth;
                customer.Firstname = model.Firstname;
                customer.Firstname = model.Lastname;
                customer.Email = model.Email;
                if (checkPhoneNumber)
                {
                    customer.PhoneNumber = model.PhoneNumber;
                }
                else
                    return BadRequest(ErrorCodeEnum.PhoneFormatError, Resource.MobileCheck, null);///

                var res = _repo.UpdateAsync(customer, cancellationToken);

                if (res.IsCanceled)
                    return BadRequest(ErrorCodeEnum.BadRequest, Resource.GeneralErrorTryAgain, null);///

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        public async Task<ServiceResult> CheckConflicts(string firstName, string lastName, DateTime dateOfBirth, string email, CancellationToken cancellationToken)
        {
            try
            {
                var conflict = _repo.TableNoTracking
                .Any(x => x.Firstname == firstName
                && x.Lastname == lastName
                && x.DateOfBirth == dateOfBirth);

                if (conflict)
                    return BadRequest(ErrorCodeEnum.BadRequest, Resource.ConflictError, null);///

                var emailCheck = _repo.TableNoTracking
                    .Any(x => x.Email == email);

                if (emailCheck)
                    return BadRequest(ErrorCodeEnum.BadRequest, Resource.EmailConflict, null);///

                return Ok(true);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }
    }
}
