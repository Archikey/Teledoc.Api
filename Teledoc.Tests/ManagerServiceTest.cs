using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Teledoc.ApiServices.DataBase.Context;
using Teledoc.ApiServices.DataBase.Entities;
using Teledoc.ApiServices.DTO.Requests;
using Teledoc.ApiServices.Repositorys.Realization;
using Teledoc.ApiServices.Services.Realization;

namespace Teledoc.Tests
{
    public class ManagerServiceTest
    {
        private readonly Mock<ILogger<ManagerService>> _mockLoggerManagerService;
        private readonly Mock<ILogger<ClientRepository>> _mockLoggerClientRepository;
        private readonly ClientRepository clientRepository;
        private readonly ManagerService managerService;
        private readonly AppDbContext _appDbContext;
        public ManagerServiceTest()
        {

            _mockLoggerManagerService = new Mock<ILogger<ManagerService>>();
            _mockLoggerClientRepository = new Mock<ILogger<ClientRepository>>();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDb2")
                    .Options;

            _appDbContext = new AppDbContext(options);
            _appDbContext.Database.EnsureDeleted();
            _appDbContext.Database.EnsureCreated();
            clientRepository = new ClientRepository(_appDbContext, _mockLoggerClientRepository.Object);
            managerService = new ManagerService(_mockLoggerManagerService.Object, clientRepository);
        }

        [Fact]
        public async Task CreateClientAsyncBadReuest()
        {
            var clientRequest = CreateClientBad();

            var result = await managerService.CreateClientAsync(clientRequest);

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Null(result.Data);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateClientAsyncIsCorrect()
        {
            var clientRequstIndividual = CreateClientIndividual();
            var clientRequestLagal = CreateClientLegal();

            var result1 = await managerService.CreateClientAsync(clientRequstIndividual);
            var result2 = await managerService.CreateClientAsync(clientRequestLagal);

            Assert.True(result1.IsSuccess);
            Assert.True(result2.IsSuccess);

            Assert.NotNull(result1.Data);
            Assert.NotNull(result2.Data);

            Assert.Equal(clientRequstIndividual.Name, result1.Data.Name);
            Assert.Equal(clientRequstIndividual.TaxpayerIndividualNumber, result1.Data.TaxpayerIndividualNumber);
            Assert.Equal(clientRequestLagal.Name, result2.Data.Name);
            Assert.Equal(clientRequestLagal.TaxpayerIndividualNumber, result2.Data.TaxpayerIndividualNumber);
            Assert.Equal(clientRequstIndividual.Founders.Count, result1.Data.Founders.Count);
            Assert.Equal(clientRequestLagal.Founders.Count, result2.Data.Founders.Count);
        }

        [Fact]
        public async Task CreateClientAsyncIsConflict()
        {
            var clientRequstIndividual = CreateClientIndividual();
            var clientRequestLagal = CreateClientLegal();

            var result1 = await managerService.CreateClientAsync(clientRequstIndividual);
            var result2 = await managerService.CreateClientAsync(clientRequstIndividual);

            Assert.False(result2.IsSuccess);
            Assert.Equal(StatusCodes.Status409Conflict, result2.StatusCode);
            Assert.Null(result2.Data);
        }

        [Fact]
        public async Task DeleteClientAsyncIsCorrect()
        {
            var clientRequest = CreateClientLegal();

            var createResult = await managerService.CreateClientAsync(clientRequest);

            var result = await managerService.DeleteClientAsync(createResult.Data!.Id);

            Assert.True(result.IsSuccess);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Null(result.Data);

            var getResult = await managerService.GetByIdClientAsync(createResult.Data.Id);

            Assert.False(getResult.IsSuccess);
            Assert.Equal(StatusCodes.Status404NotFound, getResult.StatusCode);
            Assert.Null(getResult.Data);
        }

        [Fact]
        public async Task DeleteClientAsyncNotFound()
        {
            var result = await managerService.DeleteClientAsync(999);

            Assert.False(result.IsSuccess);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetAllClientAsyncIsEmpty()
        {
            var result = await managerService.GetAllClientAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetAllClientAsyncIsCorrect()
        {
            var clientRequestIndividual = CreateClientIndividual();
            var clientRequestLegal = CreateClientLegal();

            await managerService.CreateClientAsync(clientRequestIndividual);
            await managerService.CreateClientAsync(clientRequestLegal);

            var result = await managerService.GetAllClientAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);

            Assert.Contains(result.Data, x =>
                x.Name == clientRequestIndividual.Name &&
                x.TaxpayerIndividualNumber == clientRequestIndividual.TaxpayerIndividualNumber);

            Assert.Contains(result.Data, x =>
                x.Name == clientRequestLegal.Name &&
                x.TaxpayerIndividualNumber == clientRequestLegal.TaxpayerIndividualNumber);
        }
        [Fact]
        public async Task GetByIdClientAsyncIsCorrect()
        {
            var clientRequest = CreateClientLegal();

            var createResult = await managerService.CreateClientAsync(clientRequest);

            var result = await managerService.GetByIdClientAsync(createResult.Data!.Id);

            Assert.True(result.IsSuccess);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(createResult.Data.Id, result.Data.Id);
            Assert.Equal(clientRequest.Name, result.Data.Name);
            Assert.Equal(clientRequest.TaxpayerIndividualNumber, result.Data.TaxpayerIndividualNumber);
            Assert.Equal(clientRequest.Founders.Count, result.Data.Founders.Count);
        }

        [Fact]
        public async Task GetByIdClientAsyncNotFound()
        {
            var result = await managerService.GetByIdClientAsync(999);

            Assert.False(result.IsSuccess);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetByInnClientAsyncIsCorrect()
        {
            var clientRequest = CreateClientLegal();

            var createResult = await managerService.CreateClientAsync(clientRequest);

            var result = await managerService.GetByInnClientAsync(clientRequest.TaxpayerIndividualNumber);

            Assert.True(result.IsSuccess);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(createResult.Data!.Id, result.Data.Id);
            Assert.Equal(clientRequest.Name, result.Data.Name);
            Assert.Equal(clientRequest.TaxpayerIndividualNumber, result.Data.TaxpayerIndividualNumber);
            Assert.Equal(clientRequest.Founders.Count, result.Data.Founders.Count);
        }

        [Fact]
        public async Task GetByInnClientAsyncNotFound()
        {
            var result = await managerService.GetByInnClientAsync("5005247018");

            Assert.False(result.IsSuccess);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateClientAsyncIsCorrect()
        {
            var clientRequest = CreateClientLegal();

            var createResult = await managerService.CreateClientAsync(clientRequest);

            var updateRequest = CreateClientLegal();
            updateRequest.Name = "Updated legal client";
            updateRequest.TaxpayerIndividualNumber = clientRequest.TaxpayerIndividualNumber;

            var result = await managerService.UpdateClientAsync(createResult.Data!.Id, updateRequest);

            Assert.True(result.IsSuccess);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.NotNull(result.Data);

            Assert.Equal(createResult.Data.Id, result.Data.Id);
            Assert.Equal(updateRequest.Name, result.Data.Name);
            Assert.Equal(updateRequest.TaxpayerIndividualNumber, result.Data.TaxpayerIndividualNumber);
            Assert.Equal(updateRequest.Founders.Count, result.Data.Founders.Count);
        }

        [Fact]
        public async Task UpdateClientAsyncChangeFoundersCountIsCorrect()
        {
            var clientRequest = CreateClientLegal();

            var createResult = await managerService.CreateClientAsync(clientRequest);

            var updateRequest = CreateClientLegal();
            updateRequest.TaxpayerIndividualNumber = clientRequest.TaxpayerIndividualNumber;

            updateRequest.Founders.Add(new FounderRequests
            {
                TaxpayerIndividualNumber = "884932071575",
                FirstName = "Petr",
                LastName = "Petrov",
                MiddleName = "Petrovich"
            });

            var result = await managerService.UpdateClientAsync(createResult.Data!.Id, updateRequest);

            Assert.True(result.IsSuccess);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.NotNull(result.Data);

            Assert.Equal(updateRequest.Founders.Count, result.Data.Founders.Count);
            Assert.Equal(3, result.Data.Founders.Count);
        }

        [Fact]
        public async Task UpdateClientAsyncBadRequest()
        {
            var clientRequest = CreateClientIndividual();

            var createResult = await managerService.CreateClientAsync(clientRequest);

            var updateRequest = CreateClientIndividual();

            updateRequest.Founders.Add(new FounderRequests
            {
                TaxpayerIndividualNumber = "884932071575",
                FirstName = "Petr",
                LastName = "Petrov",
                MiddleName = "Petrovich"
            });

            var result = await managerService.UpdateClientAsync(createResult.Data!.Id, updateRequest);

            Assert.False(result.IsSuccess);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Null(result.Data);
        }
        [Fact]
        public async Task UpdateClientAsyncNotFound()
        {
            var updateRequest = CreateClientLegal();

            var result = await managerService.UpdateClientAsync(999, updateRequest);

            Assert.False(result.IsSuccess);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Null(result.Data);
        }
        [Fact]
        public async Task UpdateClientAsyncIsConflict()
        {
            var firstClientRequest = CreateClientIndividual();
            var secondClientRequest = CreateClientLegal();

            var firstResult = await managerService.CreateClientAsync(firstClientRequest);
            var secondResult = await managerService.CreateClientAsync(secondClientRequest);

            var updateRequest = CreateClientLegal();
            updateRequest.TaxpayerIndividualNumber = firstClientRequest.TaxpayerIndividualNumber;

            var result = await managerService.UpdateClientAsync(secondResult.Data!.Id, updateRequest);

            Assert.False(result.IsSuccess);
            Assert.Equal(StatusCodes.Status409Conflict, result.StatusCode);
            Assert.Null(result.Data);

            Assert.True(firstResult.IsSuccess);
        }





        private ClientRequests CreateClientBad()
        {
            var clientRequests = new ClientRequests
            {
                Name = "test",
                TaxpayerIndividualNumber = "500100732259",
                TypeRole = ClientType.IndividualEntrepreneur,
            };

            clientRequests.Founders.Add(new FounderRequests
            {
                FirstName = "test",
                MiddleName = "test2",
                LastName = "test3",
                TaxpayerIndividualNumber = "500100732259"
            });
            clientRequests.Founders.Add(new FounderRequests
            {
                FirstName = "test",
                MiddleName = "test2",
                LastName = "test3",
                TaxpayerIndividualNumber = "500100732259"
            });
            return clientRequests;
        }
        private ClientRequests CreateClientIndividual()
        {
            var clientRequests = new ClientRequests
            {
                Name = "test",
                TaxpayerIndividualNumber = "500100732259",
                TypeRole = ClientType.IndividualEntrepreneur,
            };

            clientRequests.Founders.Add(new FounderRequests
            {
                FirstName = "test",
                MiddleName = "test2",
                LastName = "test3",
                TaxpayerIndividualNumber = "500100732259"
            });
            return clientRequests;
        }
        private ClientRequests CreateClientLegal()
        {
            var clientRequests = new ClientRequests
            {
                Name = "test",
                TaxpayerIndividualNumber = "5005247018",
                TypeRole = ClientType.LegalEntity,
            };

            clientRequests.Founders.Add(new FounderRequests
            {
                FirstName = "test",
                MiddleName = "test2",
                LastName = "test3",
                TaxpayerIndividualNumber = "500100732259"
            });
            clientRequests.Founders.Add(new FounderRequests
            {
                FirstName = "test4",
                MiddleName = "test5",
                LastName = "test6",
                TaxpayerIndividualNumber = "500100732259"
            });
            return clientRequests;
        }
    }
}
