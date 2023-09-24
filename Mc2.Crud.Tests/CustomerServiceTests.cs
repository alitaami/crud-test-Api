using Application.Models.Dtos;
using Application.Models.ViewModels;
using Application.Repository;
using Application.Services;
using AutoMapper;
using Castle.Core.Resource;
using Common.Resources;
using Domain.ApiResult;
using Domain.Entities;
using EstateAgentApi.Services.Base;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace Mc2.Crud.Tests
{
    public class CustomerServiceTests
    {
        private IRepository<Customer> _repo;
        private IMapper _mapper;
        private ILogger<CustomerService> _logger;

        private CustomerService _service;

        [SetUp]
        public void Setup()
        {
            _repo = Mock.Of<IRepository<Customer>>();
            _mapper = Mock.Of<IMapper>();
            _logger = Mock.Of<ILogger<CustomerService>>();

            _service = new CustomerService(_logger, _repo, _mapper);
        }
        #region GetById
        [TestCase(1)]
        public async Task GetById_ValidId_ReturnsCustomerDto(int Id)
        {
            //Arrange

            CancellationToken cancellationToken = CancellationToken.None;

            // Set up a mock customer object that you expect to be returned by the repository
            var mockCustomer = new Customer
            {
                Id = Id,
                Firstname = "ali",
                Lastname = "taami",
                DateOfBirth = new DateTime(2023, 9, 18, 12, 20, 8, 813),
                PhoneNumber = "9301327634",
                Email = "alitaami@gmail.com",
                BankAccountNumber = "5859831001081461"
            };

            // Set up a mock customer DTO object that you expect to be returned by the mapper
            var mockCustomerDto = new CustomerDto
            {
                Firstname = "ali",
                Lastname = "taami",
                DateOfBirth = new DateTime(2023, 9, 18, 12, 20, 8, 813),
                PhoneNumber = "9301327634",
                Email = "alitaami@gmail.com",
                BankAccountNumber = "5859831001081461"
            };

            // Mock the behavior of _repo.GetByIdAsync to return the mock customer
            Mock.Get(_repo)
                .Setup(repo => repo.GetByIdAsync(cancellationToken, Id))
                .ReturnsAsync(mockCustomer);

            Mock.Get(_mapper)
                .Setup(mapper => mapper.Map<CustomerDto>(mockCustomer))
                .Returns(mockCustomerDto);

            //Act
            var result = await _service.GetCustomerById(Id, cancellationToken);

            //Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            //Assert.That(result.Result, Is.EqualTo(ErrorCodeEnum.None));
            Assert.That(result.Result.ErrorMessage, Is.Null);
            Assert.That(result.Result.Errors, Is.Null);
            Assert.That(result.Data, Is.EqualTo(mockCustomerDto));
        }

        [TestCase(0)]
        public async Task GetById_InvalidId_ReturnsNotFound(/*Invalid Id*/ int Id)
        {
            //Arrange
            CancellationToken cancellationToken = CancellationToken.None;

            Mock.Get(_repo)
                .Setup(repo => repo.GetByIdAsync(cancellationToken, Id))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _service.GetCustomerById(Id, cancellationToken);

            // Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            Assert.That(result.Result.ErrorCode, Is.EqualTo(ErrorCodeEnum.NotFound));
            Assert.That(result.Data, Is.Null);
        }

        [TestCase(1)]
        public async Task GetById_ExceptionThrown_ReturnsInternalServerError(int id)
        {
            CancellationToken cancellationToken = CancellationToken.None;

            Mock.Get(_repo)
                .Setup(repo => repo.GetByIdAsync(cancellationToken, id))
                .Throws(new Exception());
            //Act
            var result = await _service.GetCustomerById(id, cancellationToken);

            //Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            //Assert.That(result.ResultCode, Is.EqualTo(ResultCodeEnum.InternalError)); 
            Assert.That(result.Result.ErrorCode, Is.EqualTo(ErrorCodeEnum.InternalError));
            Assert.That(result.Data, Is.Null);

        }
        #endregion

        #region GetAll
        [Test]
        public async Task GetCustomers_ReturnsListOfCustomerDto()
        {
            //Arrange
            var mockCustomers = new List<Customer>
         {
        new Customer
        {
                Id = 1,
                Firstname = "ali",
                Lastname = "taami",
                DateOfBirth = new DateTime(2023, 9, 18, 12, 20, 8, 813),
                PhoneNumber = "9301327634",
                Email = "alitaami@gmail.com",
                BankAccountNumber = "5859831001081461"
        },
        new Customer
        {
             Id = 2,
                 Firstname = "Ata",
                 Lastname = "Taami",
                 DateOfBirth = new DateTime(2012, 10, 11),
                 PhoneNumber = "9301327624", // Valid phone number
                 Email = "atataami91@gmail.com", // Valid email
                 BankAccountNumber = "5859831001081462" // Valid 16-digit bank account number
        }
            };

            var mockCustomersDto = mockCustomers.Select(x => new CustomerDto
            {
                Firstname = x.Firstname,
                Lastname = x.Lastname,
                DateOfBirth = x.DateOfBirth,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                BankAccountNumber = x.BankAccountNumber
            }).ToList();

            Mock.Get(_repo)
                .Setup(repo => repo.TableNoTracking)
                .Returns(mockCustomers.AsQueryable());

            Mock.Get(_mapper)
                .Setup(mapper => mapper.Map<List<CustomerDto>>(mockCustomers))
                .Returns(mockCustomersDto);

            //Act
            var result = await _service.GetCustomers();

            //Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            Assert.That(result.Data, Is.EqualTo(mockCustomersDto));
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Result.ErrorMessage, Is.Null);
            Assert.That(result.Result.Errors, Is.Null);
        }

        [Test]
        public async Task GetCustomers_NoCustomers_ReturnsNotFound()
        {
            //Arrange
            Mock.Get(_repo)
                .Setup(repo => repo.TableNoTracking)
                .Returns(Enumerable.Empty<Customer>().AsQueryable());

            //Act
            var result = await _service.GetCustomers();

            //Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            Assert.That(result.Data, Is.Null);

        }

        [Test]
        public async Task GetCustomers_ExceptionThrown_ReturnsInternalServerError()
        {
            //Arrange
            Mock.Get(_repo)
                .Setup(repo => repo.TableNoTracking)
                .Throws(new Exception());

            //Act
            var result = await _service.GetCustomers();

            //Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            Assert.That(result.Result.ErrorCode, Is.EqualTo(ErrorCodeEnum.InternalError));

        }
        #endregion

        #region DeleteById
        [TestCase(1)]
        public async Task DeleteCustomer_ValidId_Successful(int Id)
        {
            CancellationToken cancellationToken = CancellationToken.None;

            //Arrange
            Mock.Get(_repo)
                .Setup(repo => repo.GetByIdAsync(cancellationToken, Id))
                .ReturnsAsync(new Customer());

            //Act
            var result = await _service.DeleteCustomerById(Id, cancellationToken);

            //Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            Assert.That(result.Result.ErrorMessage, Is.Null);
            Assert.That(result.Result.Errors, Is.Null);

        }

        [TestCase(0)]
        public async Task DeleteCustomer_InvalidId_ReturnsNotFound(int Id)
        {
            CancellationToken cancellationToken = CancellationToken.None;

            //Arrange
            Mock.Get(_repo)
                .Setup(repo => repo.GetByIdAsync(cancellationToken, Id))
                .ReturnsAsync((Customer)null);

            //Act
            var result = await _service.DeleteCustomerById(Id, cancellationToken);

            //Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Result.ErrorCode, Is.EqualTo(ErrorCodeEnum.NotFound));

        }

        [TestCase(0)]
        public async Task DeleteCustomer_ExceptionThrown_ReturnsInternalServerError(int Id)
        {
            CancellationToken cancellationToken = CancellationToken.None;

            //Arrange
            Mock.Get(_repo)
                .Setup(repo => repo.GetByIdAsync(cancellationToken, Id))
                .Throws(new Exception());

            //Act
            var result = await _service.DeleteCustomerById(Id, cancellationToken);

            //Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            Assert.That(result.Result.ErrorCode, Is.EqualTo(ErrorCodeEnum.InternalError));
        }

        #endregion

        #region Create
        [Test]
        public async Task AddCustomer_ValidInput_Successful()
        {
            CancellationToken cancellationToken = CancellationToken.None;
          
            // Arrange
            var model = new CustomerViewModel
            {
                // Provide valid input data here
                Firstname = "John",
                Lastname = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john@example.com",
                PhoneNumber = "9301327634",
                CountryCode = "IR",
                BankAccountNumber = "58599831001081461"
            };
            Customer customer = new Customer
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BankAccountNumber = model.BankAccountNumber
            };

            Mock.Get(_repo)
                .Setup(repo => repo.AddAsync(customer, cancellationToken, true))
                .Returns(Task.CompletedTask); // Simulate a successful addition

            // Act
            var result = await _service.AddCustomer(model, CancellationToken.None);

            //Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            Assert.That(result.Result.ErrorCode, Is.EqualTo(ErrorCodeEnum.None));

        }

        [Test]
        public async Task AddCustomer_InvalidPhoneNumber_ReturnsBadRequest()
        {
            CancellationToken cancellationToken = CancellationToken.None;
         
            // Arrange
            var model = new CustomerViewModel
            {
                // Provide valid input data here
                Firstname = "John",
                Lastname = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john@example.com",
                PhoneNumber = "301327634",
                CountryCode = "IR",
                BankAccountNumber = "58599831001081461"
            };
            Customer customer = new Customer
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BankAccountNumber = model.BankAccountNumber
            };

            Mock.Get(_repo)
                .Setup(repo => repo.AddAsync(customer, cancellationToken, true))
                .Returns(Task.FromCanceled<int>(new CancellationToken(canceled: true))); // Simulate an UnSuccessful addition

            // Act
            var result = await _service.AddCustomer(model, CancellationToken.None);

            //Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            Assert.That(result.Result.ErrorCode, Is.EqualTo(ErrorCodeEnum.PhoneFormatError));
        }

        [Test]
        public async Task AddCustomer_Conflict_ReturnsBadRequest()
        {
            CancellationToken cancellationToken = CancellationToken.None;

            // Arrange
            var model = new CustomerViewModel
            {
                // Provide valid input data here
                Firstname = "John",
                Lastname = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john@example.com",
                PhoneNumber = "9301327634",
                CountryCode = "IR",
                BankAccountNumber = "58599831001081461"
            };

            Customer customer = new Customer
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BankAccountNumber = model.BankAccountNumber
            };

            // Mock the TableNoTracking  method of the repository to simulate a duplicate addition
            Mock.Get(_repo)
                .Setup(repo => repo.TableNoTracking)
                .Returns(new[] { customer }.AsQueryable());

            // Act
            var result = await _service.AddCustomer(model, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            Assert.That(result.Result.ErrorCode, Is.EqualTo(ErrorCodeEnum.UserAlreadyExists));
        }

        [Test]
        public async Task AddCustomer_InternalError_ReturnsInternalServerError()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            // Create a mock customer model
            var model = new CustomerViewModel
            {
                // Provide valid input data here
                Firstname = "John",
                Lastname = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john@example.com",
                PhoneNumber = "9301327634",
                CountryCode = "IR",
                BankAccountNumber = "58599831001081461"
            };

            // Mock the AddAsync method of the repository to throw an exception
            Mock.Get(_repo)
                .Setup(repo => repo.AddAsync(It.IsAny<Customer>(), cancellationToken, true))
                .Throws(new Exception("An error occurred during customer addition."));

            // Act
            var result = await _service.AddCustomer(model, cancellationToken);

            // Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            Assert.That(result.Result.ErrorCode, Is.EqualTo(ErrorCodeEnum.InternalError));
        }

        #endregion
    }
}