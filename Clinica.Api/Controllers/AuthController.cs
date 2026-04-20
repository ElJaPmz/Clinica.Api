using Clinica.Application.DTOs.Usuarios;
using Clinica.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDto request)
        {
            // Este método ya funciona totalmente y guarda en la DB
            var resultado = await _authService.Registrar(request);

            if (resultado.Contains("Error"))
                return BadRequest(new { mensaje = resultado });

            return Ok(new { mensaje = resultado });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto request)
        {
            var resultado = await _authService.Login(request);

            // Si las credenciales fallan, el servicio devuelve null
            if (resultado == null)
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });

            // Si es correcto, devuelve el DTO que ya contiene el Token
            return Ok(resultado);
        }
    }
}