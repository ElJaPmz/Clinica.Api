using Clinica.Application.DTOs.Usuarios;
using Clinica.Application.Response;

namespace Clinica.Application.Interfaces.Service
{
    public interface IUsuarioService
    {
        // Gestión de Autenticación
        Task<LoginRespuestaUsuarioDto> LoginAsync(UsuarioLoginDto dto);

        // Gestión de Cuentas
        Task<UsuarioDto> RegistrarUsuario(UsuarioRegistroDto dto);
        Task CambiarEstadoAsync(string id, bool activo);

        // Consultas de Usuarios
        Task<ICollection<UsuarioDto>> ObtenerUsuariosAsync(int pagina, int tamanoPagina);
        Task<IEnumerable<UsuarioDto>> BuscarUsuarioAsync(string valor, int pagina, int tamanoPagina);
        Task<UsuarioDto> ObtenerUsuarioPorIdAsync(string id);

        // Métodos de apoyo para paginación (Estilo StayInn)
        Task<int> ContarAsync();
        Task<int> ContarBusquedaAsync(string valor);
    }
}