using Clinica.Application.Interface.Service;
using Clinica.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Clinica.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string CrearToken(Usuario usuario)
        {
            // 1. Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id_Usuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            // 2. Llave (Leída de appsettings.json)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            // 3. Credenciales de firma 
            // CAMBIO: Usamos HmacSha256 para máxima compatibilidad con el middleware por defecto de .NET
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // 4. Descriptor del token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                // CAMBIO: Usar UtcNow para evitar desfases horarios que invaliden el token al nacer
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = creds,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            // 5. Generación
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}