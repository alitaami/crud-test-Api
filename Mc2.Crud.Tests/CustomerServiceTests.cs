using Application.Models.Dtos;
using Application.Repository;
using Application.Services;
using AutoMapper;
using Castle.Core.Resource;
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
        public async Task GetCustomerById_WithValidId_ReturnsCustomerDto(int Id)
        {
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
            Assert.That(result.Data, Is.EqualTo(mockCustomerDto));
        }

        [TestCase(0)]
        public async Task GetCustomerById_WithInValidId_ReturnsNotFound(/*Invalid Id*/ int Id)
        {
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await _service.GetCustomerById(Id, cancellationToken);

            // Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            //Assert.That(result, Is.EqualTo(ErrorCodeEnum.NotFound)); // Check the expected result code
            Assert.That(result.Data, Is.Null);
        }

        [TestCase(1)]
        public async Task GetCustomerById_WithException_ReturnsInternalServerError(int id)
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
            Assert.That(result.Data, Is.Null);

        }
        #endregion

        #region GetAll
        [Test]
        public async Task GetCustomers_ReturnsListOfCustomerDto()
        {
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

            var result = await _service.GetCustomers();

            //Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public async Task GetCustomers_ReturnsNotFound()
        {
            Mock.Get(_repo)
                .Setup(repo => repo.TableNoTracking)
                .Returns(Enumerable.Empty<Customer>().AsQueryable());

            var result = await _service.GetCustomers();

            //Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
            Assert.That(result.Data, Is.Null);
        }

        [Test]
        public async Task GetCustomers_ReturnsInternalServerError()
        {
            Mock.Get(_repo)
                .Setup(repo => repo.TableNoTracking)
                .Throws(new Exception());

            //Act
            var result = await _service.GetCustomers();

            //Assert
            Assert.That(result, Is.InstanceOf<ServiceResult>());
        }
        #endregion
    }
}