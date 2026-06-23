using Clinica.Domain.Entities;

namespace Clinica.Application.Interfaces.Persistence
{
    public interface IUsuarioRepository
    {
        // Obtiene un usuario por su ID (string debido a IdentityUser)
        Task<ApplicationUser?> ObtenerPorIdAsync(string id);

        // Realiza una búsqueda paginada de usuarios
        Task<IEnumerable<ApplicationUser>> BuscarUsuarioAsync(string valor, int pagina, int tamanoPagina);

        // Obtiene la lista completa de usuarios de forma paginada
        Task<IEnumerable<ApplicationUser>> ObtenerTodosAsync(int pagina, int tamanoPagina);

        // Cuenta el total de usuarios en la base de datos
        Task<int> ContarAsync();

        // Cuenta el total de resultados encontrados en una búsqueda específica
        Task<int> ContarBusquedaAsync(string valor);
    }
}
