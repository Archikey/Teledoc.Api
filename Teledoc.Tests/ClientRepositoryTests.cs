using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Teledoc.ApiServices.DataBase.Context;
using Teledoc.ApiServices.DataBase.Entities;
using Teledoc.ApiServices.Repositorys.Realization;

namespace Teledoc.Tests
{
    public class ClientRepositoryTests
    {
        private readonly Mock<ILogger<ClientRepository>> _mockLogger;
        private readonly ClientRepository clientRepository;
        private readonly AppDbContext _appDbContext;
        public ClientRepositoryTests()
        {
            _mockLogger = new Mock<ILogger<ClientRepository>>();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDb")
                    .Options;

            _appDbContext = new AppDbContext(options);
            _appDbContext.Database.EnsureDeleted();
            _appDbContext.Database.EnsureCreated();
            clientRepository = new ClientRepository(_appDbContext, _mockLogger.Object);

        }

        [Fact]
        public async Task TestAddAsyncIsCorrect()
        {
            var client = CreateClient();

           await clientRepository.AddAsync(client);

            var result = await _appDbContext.Clients
                .Include(x => x.Founders)
                .FirstOrDefaultAsync(x => x.Id == client.Id);

            Assert.NotNull(result);
            Assert.Equal(client.Id, result.Id);
         
    
        }

        [Fact]
        public async Task TestGetByIdAsyncIsCorrect()
        {
            var client = CreateClient();

            await clientRepository.AddAsync(client);

            var result = await clientRepository.GetByIdAsync(client.Id);

            Assert.NotNull(result);
            Assert.Equal(client.Id, result.Id);
       
        }


        [Fact]
        public async Task TestGetByInnAsyncIsCorrect()
        {
            var client = CreateClient();

            await clientRepository.AddAsync(client);

            var result = await clientRepository.GetByInnAsync(client.TaxpayerIndividualNumber);

            Assert.NotNull(result);
            Assert.Equal(client.Id, result.Id);
       
            Assert.Equal(client.TaxpayerIndividualNumber, result.TaxpayerIndividualNumber);
        }


        [Fact]
        public async Task TestUpdateAsyncIsCorrect()
        {
            var client = CreateClient();

            await clientRepository.AddAsync(client);

            client.Name = "Updated name";
            client.UpdatedAt = DateTime.UtcNow;

            await clientRepository.UpdateAsync(client);

            var result = await clientRepository.GetByIdAsync(client.Id);

            Assert.NotNull(result);
            Assert.Equal(client.Id, result.Id);
            Assert.Equal("Updated name", result.Name);
        }

        [Fact]
        public async Task TestDeletedAsyncIsCorrect()
        {
            var client = CreateClient();

            await clientRepository.AddAsync(client);
            await clientRepository.DeleteAsync(client);

            var result = await clientRepository.GetByInnAsync(client.TaxpayerIndividualNumber);

            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetByAllAsyncIsCorrect()
        {
            var client = CreateClient();

            await clientRepository.AddAsync(client);

            var result = await clientRepository.GetAllAsync();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(client.TaxpayerIndividualNumber, result[0].TaxpayerIndividualNumber);
        }

        private Client CreateClient()
        {
            var now = DateTime.UtcNow;

            var client = new Client
            {
                Id = 1,
                Name = "Test Legal Entity",
                TaxpayerIndividualNumber = "5005247018",
                TypeRole = "LegalEntity",
                CreatedAt = now,
                UpdatedAt = now,
                Founders = []
            };

            client.Founders.Add(new Founder
            {
                Id = 1,
                TaxpayerIndividualNumber = "500100732259",
                FirstName = "Ivan",
                MiddleName = "Ivanovich",
                LastName = "Ivanov",
                CreatedAt = now,
                UpdatedAt = now,
                ClientId = client.Id,
                Client = client
            });

            return client;
        }
    }
}
