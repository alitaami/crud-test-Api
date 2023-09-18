using Application.Models.Dtos;
using Application.Models.ViewModels;
using Domain.ApiResult;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<ServiceResult> GetCustomerById(int id, CancellationToken cancellationToken);
        public Task<ServiceResult> DeleteCustomerById(int id, CancellationToken cancellationToken);
        public Task<ServiceResult> GetCustomers();
        public Task<ServiceResult> AddCustomer(CustomerViewModel model, CancellationToken cancellationToken);
        public Task<ServiceResult> UpdateCustomer( CustomerUpdateViewModel model, CancellationToken cancellationToken);
        public Task<ServiceResult> CheckConflicts( string firstName,string lastName,DateTime dateOfBirth,string email, CancellationToken cancellationToken);


    }
}
