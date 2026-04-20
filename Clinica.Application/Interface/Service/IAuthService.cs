using Clinica.Application.DTOs.Usuarios;

namespace Clinica.Application.Interfaces
{
    public interface IAuthService
    {
        // Se mantiene igual para el registro
        Task<string> Registrar(UsuarioRegistroDto request);

        // CAMBIO: Ahora devuelve un UsuarioRespuestaDto (que crearemos a continuación)
        // El "?" es porque si las credenciales son malas, devolverá null
        Task<UsuarioRespuestaDto?> Login(UsuarioLoginDto request);
    }
}
