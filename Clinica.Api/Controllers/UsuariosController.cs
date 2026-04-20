using Clinica.Application.DTOs;
using Clinica.Application.DTOs.Usuarios;
using Clinica.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Clinica.Api.Controllers
{
    [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // 1. LISTAR TODOS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.ObtenerTodos();
            return Ok(usuarios);
        }

        // 2. OBTENER UNO POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _usuarioService.ObtenerPorId(id);
            if (usuario == null) return NotFound($"No se encontró el usuario con ID {id}");
            return Ok(usuario);
        }

        // 3. ACTUALIZAR (Nombre y Rol)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioDto usuarioDto)
        {
            var resultado = await _usuarioService.Actualizar(id, usuarioDto);
            if (!resultado) return BadRequest("No se pudo actualizar el usuario. Verifica si el ID existe.");

            return Ok("Usuario actualizado correctamente.");
        }

        // 4. ELIMINAR
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _usuarioService.Eliminar(id);
            if (!resultado) return BadRequest("Error al intentar eliminar el usuario.");

            return Ok("Usuario eliminado con éxito.");
        }
    }
}