using Microsoft.AspNetCore.Mvc;
using Teledoc.ApiServices.DTO.Requests;
using Teledoc.ApiServices.Services.Interfaces;

namespace Teledoc.ApiServices.Controllers
{

    /// <summary>
    /// Контроллер для управления клиентами
    /// </summary>
    [Route("api/v1/clients")]
    [ApiController]
    public class ClientManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public ClientManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        /// <summary>
        /// Получить список всех клиентов
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Список клиентов</returns>
        [HttpGet]
        public async Task<IActionResult> GetClients(CancellationToken cancellationToken)
        {
            var result = await _managerService.GetAllClientAsync(cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Получить клиента по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Клиент</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetClientById(int id, CancellationToken cancellationToken)
        {
            var result = await _managerService.GetByIdClientAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Получить клиента по ИНН
        /// </summary>
        /// <param name="inn">ИНН клиента</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Клиент</returns>
        [HttpGet("by-inn/{inn}")]
        public async Task<IActionResult> GetClientByInn(string inn, CancellationToken cancellationToken)
        {
            var result = await _managerService.GetByInnClientAsync(inn, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Создать клиента
        /// </summary>
        /// <param name="clientRequests">Данные клиента</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Созданный клиент</returns>
        [HttpPost]
        public async Task<IActionResult> CreateClient(
            [FromBody] ClientRequests clientRequests,
            CancellationToken cancellationToken)
        {
            var result = await _managerService.CreateClientAsync(clientRequests, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Обновить данные клиента
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <param name="clientRequests">Новые данные клиента</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Обновлённый клиент</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateClient(
            int id,
            [FromBody] ClientRequests clientRequests,
            CancellationToken cancellationToken)
        {
            var result = await _managerService.UpdateClientAsync(id, clientRequests, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Удалить клиента по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Результат удаления клиента</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteClient(int id, CancellationToken cancellationToken)
        {
            var result = await _managerService.DeleteClientAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
