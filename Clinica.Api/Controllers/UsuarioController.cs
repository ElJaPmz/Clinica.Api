using Clinica.Application.DTOs.Usuarios;
using Clinica.Application.Interfaces.Service;
using Clinica.Application.Response; // Asegúrate de tener esta clase para la paginación
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        // Obtener todos los usuarios con paginación
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> ObtenerTodos([FromQuery] int numeroPagina = 1, [FromQuery] int tamanoPagina = 10)
        {
            var usuarios = await _service.ObtenerUsuariosAsync(numeroPagina, tamanoPagina);
            var totalUsuarios = await _service.ContarAsync();

            // Usamos la respuesta paginada al estilo del profe
            return Ok(new RespuestaPaginada<UsuarioDto>(usuarios, totalUsuarios, numeroPagina, tamanoPagina));
        }

        // Buscar usuarios por nombre o email
        [HttpGet("buscar")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> BuscarUsuario([FromQuery] string valor, [FromQuery] int numeroPagina = 1, [FromQuery] int tamanoPagina = 10)
        {
            var usuarios = await _service.BuscarUsuarioAsync(valor, numeroPagina, tamanoPagina);
            var totalUsuarios = await _service.ContarBusquedaAsync(valor);

            return Ok(new RespuestaPaginada<UsuarioDto>(usuarios, totalUsuarios, numeroPagina, tamanoPagina));
        }

        // Obtener un usuario específico por ID
        [HttpGet("{id}", Name = "ObtenerUsuario")]
        [Authorize] // Cualquier usuario autenticado puede consultar su perfil si manejas la lógica, o déjalo en Administrador
        public async Task<IActionResult> ObtenerPorId(string id)
        {
            var usuario = await _service.ObtenerUsuarioPorIdAsync(id);
            return Ok(usuario);
        }

        // Registro de nuevos usuarios
        [HttpPost("registro")]
        [AllowAnonymous] // Permitir que cualquiera se registre o cambiar a [Authorize(Roles = "Administrador")] según tu regla
        public async Task<ActionResult> RegistrarUsuario([FromBody] UsuarioRegistroDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _service.RegistrarUsuario(dto);

            // CreatedAtRoute ayuda a que la respuesta incluya la URL del nuevo recurso
            return CreatedAtRoute("ObtenerUsuario", new { id = resultado.Id }, resultado);
        }

        // Login de usuario
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginRespuestaUsuarioDto>> Login([FromBody] UsuarioLoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var respuesta = await _service.LoginAsync(dto);
            return Ok(respuesta);
        }

        // Cambiar estado (Activar/Desactivar)
        [HttpPatch("{id}/estado")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> CambiarEstado(string id, [FromBody] bool estado)
        {
            await _service.CambiarEstadoAsync(id, estado);
            return NoContent();
        }
    }
}
