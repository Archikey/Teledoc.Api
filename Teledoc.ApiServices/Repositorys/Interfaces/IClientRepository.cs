using Teledoc.ApiServices.DataBase.Entities;

namespace Teledoc.ApiServices.Repositorys.Interfaces
{
    public interface IClientRepository
    {
        Task<Client?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Client?> GetByInnAsync(string inn, CancellationToken cancellationToken = default);

        Task<List<Client>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(Client client, CancellationToken cancellationToken = default);

        Task UpdateAsync(Client client, CancellationToken cancellationToken = default);

        Task DeleteAsync(Client client, CancellationToken cancellationToken = default);
    }
}
