using Teledoc.ApiServices.DTO.Requests;
using Teledoc.ApiServices.DTO.Responses;

namespace Teledoc.ApiServices.Services.Interfaces
{
    public interface IManagerService
    {
        public Task<MessageResponse<List<ClientResponse>>> GetAllClientAsync(CancellationToken cancellationToken = default);
        public Task<MessageResponse<ClientResponse>> GetByInnClientAsync(string inn, CancellationToken cancellationToken = default);
        public Task<MessageResponse<ClientResponse>> GetByIdClientAsync(int id, CancellationToken cancellationToken = default);
        public Task<MessageResponse<ClientResponse>> CreateClientAsync(ClientRequests clientRequests, CancellationToken cancellationToken = default);
        public Task<MessageResponse<ClientResponse>> UpdateClientAsync(int id, ClientRequests clientRequests, CancellationToken cancellationToken = default);
        public Task<MessageResponse<ClientResponse>> DeleteClientAsync(int id, CancellationToken cancellationToken = default);
    }
}
