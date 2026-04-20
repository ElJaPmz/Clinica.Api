using Clinica.Application.DTOs.Usuarios;
using Clinica.Application.Interfaces;
using Clinica.Infrastructure.Interfaces.Persistencia;
using Clinica.Domain.Entities;
using Clinica.Application.Interface.Service;

namespace Clinica.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtService _jwtService; // Inyectamos el servicio de JWT

        public AuthService(IUsuarioRepository usuarioRepository, IJwtService jwtService)
        {
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
        }

        public async Task<string> Registrar(UsuarioRegistroDto request)
        {
            var usuarioExistente = await _usuarioRepository.GetUsuarioByNombre(request.NombreUsuario);
            if (usuarioExistente != null)
            {
                return "Error: El nombre de usuario ya está en uso.";
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var nuevoUsuario = new Usuario
            {
                NombreUsuario = request.NombreUsuario,
                PasswordHash = passwordHash,
                Rol = request.Rol,
                Id_Medico = request.Id_Medico
            };

            await _usuarioRepository.AddUsuario(nuevoUsuario);
            var resultado = await _usuarioRepository.SaveChangesAsync();

            return resultado ? "Usuario registrado exitosamente." : "Error al guardar el usuario.";
        }

        // Corregido: Ahora devuelve UsuarioRespuestaDto para entregar el Token
        public async Task<UsuarioRespuestaDto?> Login(UsuarioLoginDto request)
        {
            // 1. Buscar al usuario
            var usuario = await _usuarioRepository.GetUsuarioByNombre(request.NombreUsuario);

            // Si no existe, devolvemos null
            if (usuario == null) return null;

            // 2. Verificar si la contraseña coincide
            bool passwordValida = BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash);

            // Si la contraseña no es válida, devolvemos null
            if (!passwordValida) return null;

            // 3. Si todo es correcto, generamos el Token y devolvemos el DTO completo
            return new UsuarioRespuestaDto
            {
                NombreUsuario = usuario.NombreUsuario,
                Rol = usuario.Rol,
                Token = _jwtService.CrearToken(usuario) // Aquí usamos el servicio que creamos
            };
        }
    }
}