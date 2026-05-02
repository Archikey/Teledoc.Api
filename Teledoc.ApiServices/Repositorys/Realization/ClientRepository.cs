using Microsoft.EntityFrameworkCore;
using Teledoc.ApiServices.DataBase.Context;
using Teledoc.ApiServices.DataBase.Entities;
using Teledoc.ApiServices.Repositorys.Interfaces;

namespace Teledoc.ApiServices.Repositorys.Realization
{
    public sealed class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ClientRepository> _logger;

        public ClientRepository(AppDbContext context, ILogger<ClientRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Client?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Clients
                .Include(x => x.Founders)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Client?> GetByInnAsync(string inn, CancellationToken cancellationToken = default)
        {
            return await _context.Clients
                .Include(x => x.Founders)
                .FirstOrDefaultAsync(x => x.TaxpayerIndividualNumber == inn, cancellationToken);
        }

        public async Task<List<Client>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Clients
                .Include(x => x.Founders)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Client client, CancellationToken cancellationToken = default)
        {
            await _context.Clients.AddAsync(client, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("The client was added");
        }

        public async Task UpdateAsync(Client client, CancellationToken cancellationToken = default)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("The client was updated");
        }

        public async Task DeleteAsync(Client client, CancellationToken cancellationToken = default)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("The client was deleted");
        }
    }
}
