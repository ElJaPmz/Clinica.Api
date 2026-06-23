using AutoMapper;
using Clinica.Application.DTOs.Usuarios;
using Clinica.Application.Interfaces.Persistence;
using Clinica.Application.Interfaces.Service;
using Clinica.Application.Response;
using Clinica.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Clinica.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsuarioService(
            IUsuarioRepository repository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IConfiguration config)
        {
            _repository = repository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _config = config;
        }

        #region Métodos Privados

        private async Task<UsuarioDto> MapearUsuarioDTOAsync(ApplicationUser usuario)
        {
            var roles = await _userManager.GetRolesAsync(usuario);

            return new UsuarioDto
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email ?? string.Empty,
                Rol = roles.FirstOrDefault() ?? "Sin Rol",
                PhoneNumber = usuario.PhoneNumber ?? string.Empty,
                Activo = usuario.Activo
                // Si tu DTO tiene Id_Medico, inclúyelo aquí:
                // Id_Medico = usuario.Id_Medico
            };
        }

        private static void ValidarResultado(IdentityResult resultado, string mensajeError)
        {
            if (!resultado.Succeeded)
            {
                var errores = string.Join(" | ", resultado.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"{mensajeError}: '{errores}'");
            }
        }

        private string GenerarToken(ApplicationUser usuario, string rol)
        {
            // Variables de entorno (Configuradas en tu Program.cs)
            var key = _config["JWT_KEY"]
                ?? throw new Exception("JWT_KEY no está configurada.");
            var issuer = _config["JWT_ISSUER"] ?? "ClinicaApi";
            var audience = _config["JWT_AUDIENCE"] ?? "ClinicaUser";

            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto ?? string.Empty),
                new Claim(ClaimTypes.Email, usuario.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, rol),
                new Claim("Id_Medico", usuario.Id_Medico?.ToString() ?? string.Empty), // Importante para tu lógica de clínica
                new Claim("Activo", usuario.Activo.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Estándar de Postgres/JWT
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        #endregion

        public async Task<IEnumerable<UsuarioDto>> BuscarUsuarioAsync(string valor, int pagina, int tamanoPagina)
        {
            var usuarios = await _repository.BuscarUsuarioAsync(valor, pagina, tamanoPagina);
            return _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
        }

        public async Task CambiarEstadoAsync(string id, bool activo)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            usuario.Activo = activo;
            var resultado = await _userManager.UpdateAsync(usuario);
            ValidarResultado(resultado, "Error al actualizar el estado");
        }

        public async Task<int> ContarAsync() => await _repository.ContarAsync();

        public async Task<int> ContarBusquedaAsync(string valor) => await _repository.ContarBusquedaAsync(valor);

        public async Task<LoginRespuestaUsuarioDto> LoginAsync(UsuarioLoginDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var usuario = await _userManager.FindByEmailAsync(dto.Email);

            if (usuario == null)
                throw new UnauthorizedAccessException("Credenciales incorrectas.");

            if (!usuario.Activo)
                throw new UnauthorizedAccessException("La cuenta está desactivada. Contacte al administrador.");

            if (!await _userManager.CheckPasswordAsync(usuario, dto.Password))
                throw new UnauthorizedAccessException("Credenciales incorrectas.");

            var usuarioDto = await MapearUsuarioDTOAsync(usuario);

            return new LoginRespuestaUsuarioDto
            {
                Usuario = usuarioDto,
                Token = GenerarToken(usuario, usuarioDto.Rol)
            };
        }

        public async Task<UsuarioDto> ObtenerUsuarioPorIdAsync(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) throw new KeyNotFoundException("Usuario no encontrado.");
            return await MapearUsuarioDTOAsync(usuario);
        }

        public async Task<ICollection<UsuarioDto>> ObtenerUsuariosAsync(int pagina, int tamanoPagina)
        {
            var usuarios = await _repository.ObtenerTodosAsync(pagina, tamanoPagina);
            var lista = new List<UsuarioDto>();

            foreach (var usuario in usuarios)
            {
                lista.Add(await MapearUsuarioDTOAsync(usuario));
            }

            return lista;
        }

        public async Task<UsuarioDto> RegistrarUsuario(UsuarioRegistroDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var existeEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existeEmail != null)
                throw new InvalidOperationException("El email ya está registrado.");

            // Validar/Crear Rol
            if (!await _roleManager.RoleExistsAsync(dto.Rol))
            {
                var resultadoRol = await _roleManager.CreateAsync(new IdentityRole(dto.Rol));
                ValidarResultado(resultadoRol, "Error al crear el rol");
            }

            var usuario = _mapper.Map<ApplicationUser>(dto);
            usuario.UserName = dto.Email; // Identity requiere UserName obligatoriamente
            usuario.EmailConfirmed = true;

            var usuarioCreado = await _userManager.CreateAsync(usuario, dto.Password);
            ValidarResultado(usuarioCreado, "Error al crear el usuario");

            var rolAsignado = await _userManager.AddToRoleAsync(usuario, dto.Rol);
            ValidarResultado(rolAsignado, "Error al asignar el rol");

            return await MapearUsuarioDTOAsync(usuario);
        }
    }
}
