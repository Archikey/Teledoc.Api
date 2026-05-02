using Teledoc.ApiServices.DataBase.Entities;
using Teledoc.ApiServices.DTO.Requests;
using Teledoc.ApiServices.DTO.Responses;
using Teledoc.ApiServices.Repositorys.Interfaces;

using Teledoc.ApiServices.Services.Interfaces;

namespace Teledoc.ApiServices.Services.Realization
{
    public class ManagerService : IManagerService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ManagerService> _logger;

        public ManagerService(ILogger<ManagerService> logger, IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _logger = logger;

        }


        public async Task<MessageResponse<ClientResponse>> CreateClientAsync(
            ClientRequests clientRequests,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("The creation of the client has begun");

            if (clientRequests.TypeRole == ClientType.IndividualEntrepreneur &&
                clientRequests.Founders.Count > 1)
            {
                return CreateInforamtionMessage(
                    null!,
                    StatusCodes.Status400BadRequest,
                    false,
                    "Individual entrepreneur can have only one founder");
            }

            var client = ClientRequestMapToClient(clientRequests);

            _logger.LogInformation("Customer availability check");

            var existingClient = await _clientRepository.GetByInnAsync(
                client.TaxpayerIndividualNumber,
                cancellationToken);

            if (existingClient is not null)
            {
                _logger.LogInformation("The client already exists");

                return CreateInforamtionMessage(
                    null!,
                    StatusCodes.Status409Conflict,
                    false,
                    "The client already exists");
            }

            try
            {
                await _clientRepository.AddAsync(client, cancellationToken);

                _logger.LogInformation("The client was successfully created");

                var clientResponse = await _clientRepository.GetByIdAsync(
                    client.Id,
                    cancellationToken);

                if (clientResponse is not null)
                {
                    return CreateInforamtionMessage(
                        ClientMapToClientResponse(clientResponse),
                        StatusCodes.Status201Created,
                        true,
                        "The client was successfully created");
                }

                return CreateInforamtionMessage(
                    null!,
                    StatusCodes.Status500InternalServerError,
                    false,
                    "The client was not created");
            }
            catch (Exception)
            {
                _logger.LogError("A server error has occurred while creating client");

                return CreateInforamtionMessage(
                    null!,
                    StatusCodes.Status500InternalServerError,
                    false,
                    "A server error has occurred");
            }
        }

        public async Task<MessageResponse<ClientResponse>> DeleteClientAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("The removal of the client has begun");

            var client = await _clientRepository.GetByIdAsync(id, cancellationToken);

            if (client is null)
            {
                _logger.LogInformation("Client not found");

                return CreateInforamtionMessage(
                    null!,
                    StatusCodes.Status404NotFound,
                    false,
                    "Client not found");
            }

            try
            {
                await _clientRepository.DeleteAsync(client, cancellationToken);

                _logger.LogInformation("The client has been deleted");

                return CreateInforamtionMessage(
                    null!,
                    StatusCodes.Status200OK,
                    true,
                    "The client has been deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"A server error has occurred while deleting client: {ex.Message}");

                return CreateInforamtionMessage(
                    null!,
                    StatusCodes.Status500InternalServerError,
                    false,
                    "A server error has occurred");
            }
        }

        public async Task<MessageResponse<List<ClientResponse>>> GetAllClientAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting all clients has begun");

            try
            {
                var clients = await _clientRepository.GetAllAsync(cancellationToken);

                var result = clients
                    .Select(ClientMapToClientResponse)
                    .ToList();

                _logger.LogInformation("Clients have been received");

                return CreateInforamtionMessageList(
                    result,
                    StatusCodes.Status200OK,
                    true,
                    "All clients");
            }
            catch (Exception ex)
            {
                _logger.LogError($"A server error has occurred while getting clients: {ex.Message}");

                return CreateInforamtionMessageList(
                    null!,
                    StatusCodes.Status500InternalServerError,
                    false,
                    "A server error has occurred");
            }
        }

        public async Task<MessageResponse<ClientResponse>> GetByIdClientAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting client by id has begun");

            try
            {
                var client = await _clientRepository.GetByIdAsync(id, cancellationToken);

                if (client is null)
                {
                    _logger.LogInformation("Client not found");

                    return CreateInforamtionMessage(
                        null!,
                        StatusCodes.Status404NotFound,
                        false,
                        "Client not found");
                }

                _logger.LogInformation("Client has been received");

                return CreateInforamtionMessage(
                    ClientMapToClientResponse(client),
                    StatusCodes.Status200OK,
                    true,
                    "Client has been received");
            }
            catch (Exception ex)
            {
                _logger.LogError($"A server error has occurred while getting client by id, {ex.Message}");

                return CreateInforamtionMessage(
                    null!,
                    StatusCodes.Status500InternalServerError,
                    false,
                    "A server error has occurred");
            }
        }

        public async Task<MessageResponse<ClientResponse>> GetByInnClientAsync(string inn, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting client by INN has begun");

            try
            {
                var client = await _clientRepository.GetByInnAsync(inn, cancellationToken);

                if (client is null)
                {
                    _logger.LogInformation("Client not found");

                    return CreateInforamtionMessage(
                        null!,
                        StatusCodes.Status404NotFound,
                        false,
                        "Client not found");
                }

                _logger.LogInformation("Client has been received");

                return CreateInforamtionMessage(
                    ClientMapToClientResponse(client),
                    StatusCodes.Status200OK,
                    true,
                    "Client has been received");
            }
            catch (Exception ex)
            {
                _logger.LogError($"A server error has occurred while getting client by INN {ex.Message}");

                return CreateInforamtionMessage(
                    null!,
                    StatusCodes.Status500InternalServerError,
                    false,
                    "A server error has occurred");
            }
        }

        public async Task<MessageResponse<ClientResponse>> UpdateClientAsync(int id, ClientRequests clientRequests, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updating client has begun");

            if (clientRequests.TypeRole == ClientType.IndividualEntrepreneur &&
                clientRequests.Founders.Count > 1)
            {
                return CreateInforamtionMessage(
                    null!,
                    StatusCodes.Status400BadRequest,
                    false,
                    "Individual entrepreneur can have only one founder");
            }
            var client = await _clientRepository.GetByIdAsync(id, cancellationToken);

            if (client is null)
            {
                _logger.LogInformation("Client not found");

                return CreateInforamtionMessage(
                    null!,
                    StatusCodes.Status404NotFound,
                    false,
                    "Client not found");
            }
            var existingClient = await _clientRepository.GetByInnAsync(
                clientRequests.TaxpayerIndividualNumber,
                cancellationToken);

            if (existingClient is not null && existingClient.Id != id)
            {
                _logger.LogInformation("The client already exists");

                return CreateInforamtionMessage(
                    null!,
                    StatusCodes.Status409Conflict,
                    false,
                    "The client already exists");
            }

            try
            {
                UpdateClientFromClientRequests(client, clientRequests);

                await _clientRepository.UpdateAsync(client, cancellationToken);

                var updatedClient = await _clientRepository.GetByIdAsync(id, cancellationToken);

                if (updatedClient is null)
                {
                    return CreateInforamtionMessage(
                        null!,
                        StatusCodes.Status500InternalServerError,
                        false,
                        "Client was updated, but could not be loaded");
                }

                _logger.LogInformation("Client has been updated");

                return CreateInforamtionMessage(
                    ClientMapToClientResponse(updatedClient),
                    StatusCodes.Status200OK,
                    true,
                    "Client has been updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"A server error has occurred while updating client: {ex.Message}");

                return CreateInforamtionMessage(
                    null!,
                    StatusCodes.Status500InternalServerError,
                    false,
                    "A server error has occurred");
            }
        }

        private void UpdateClientFromClientRequests(Client client, ClientRequests clientRequests)
        {
            var now = DateTime.UtcNow;

            client.Name = clientRequests.Name;
            client.TaxpayerIndividualNumber = clientRequests.TaxpayerIndividualNumber;
            client.TypeRole = clientRequests.TypeRole.ToString();
            client.UpdatedAt = now;

            client.Founders.Clear();

            foreach (var founderRequest in clientRequests.Founders)
            {
                client.Founders.Add(new Founder
                {
                    TaxpayerIndividualNumber = founderRequest.TaxpayerIndividualNumber,
                    FirstName = founderRequest.FirstName,
                    LastName = founderRequest.LastName,
                    MiddleName = founderRequest.MiddleName,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }
        }


        private MessageResponse<ClientResponse> CreateInforamtionMessage(
            ClientResponse clientResponse, int statusCode, bool isSuccess, string message) =>
            new MessageResponse<ClientResponse>()
            {
                StatusCode = statusCode,
                Data = clientResponse,
                IsSuccess = isSuccess,
                Message = message
            };

        private MessageResponse<List<ClientResponse>> CreateInforamtionMessageList(
            List<ClientResponse> clientResponse, int statusCode, bool isSuccess, string message) =>
            new MessageResponse<List<ClientResponse>>()
            {
                StatusCode = statusCode,
                Data = clientResponse,
                IsSuccess = isSuccess,
                Message = message
            };

        private Client ClientRequestMapToClient(ClientRequests request)
        {
            var now = DateTime.UtcNow;

            var client = new Client
            {
                Name = request.Name,
                TaxpayerIndividualNumber = request.TaxpayerIndividualNumber,
                TypeRole = request.TypeRole.ToString(),
                CreatedAt = now,
                UpdatedAt = now,
                Founders = []
            };

            foreach (var founder in request.Founders)
            {
                client.Founders.Add(new Founder
                {
                    TaxpayerIndividualNumber = founder.TaxpayerIndividualNumber,
                    FirstName = founder.FirstName,
                    LastName = founder.LastName,
                    MiddleName = founder.MiddleName,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }

            return client;
        }

        private ClientResponse ClientMapToClientResponse(Client client)
        {
            return new ClientResponse
            {
                Id = client.Id,
                Name = client.Name,
                TaxpayerIndividualNumber = client.TaxpayerIndividualNumber,
                TypeRole = Enum.Parse<ClientType>(client.TypeRole),
                CreatedAt = client.CreatedAt,
                UpdatedAt = client.UpdatedAt,
                Founders = client.Founders.Select(x => new FounderResponse
                {
                    Id = x.Id,
                    TaxpayerIndividualNumber = x.TaxpayerIndividualNumber,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    MiddleName = x.MiddleName,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToList()
            };
        }
    }
}
