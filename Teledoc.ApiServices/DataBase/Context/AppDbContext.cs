using Microsoft.EntityFrameworkCore;

namespace Teledoc.ApiServices.DataBase.Context
{
    /// <summary>
    /// Контекст базы данных приложения
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Инициализирует новый экземпляр контекста базы данных
        /// </summary>
        /// <param name="options">Параметры конфигурации DbContext</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
