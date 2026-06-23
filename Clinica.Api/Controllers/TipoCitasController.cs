using Clinica.Application.DTOs.TipoCita;
using Clinica.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TipoCitasController : ControllerBase
    {
        private readonly ITipoCitaService _tipoCitaService;

        public TipoCitasController(ITipoCitaService tipoCitaService)
        {
            _tipoCitaService = tipoCitaService;
        }

        /// <summary>
        /// Retorna todos los tipos de cita disponibles.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoCitaDto>>> GetTodos()
        {
            var tipos = await _tipoCitaService.ObtenerTodosAsync();
            return Ok(tipos);
        }

        /// <summary>
        /// Retorna los tipos de cita permitidos para una especialidad específica.
        /// Usar este endpoint para cargar el dropdown de Tipo de Cita luego de seleccionar un médico.
        /// </summary>
        [HttpGet("especialidad/{idEspecialidad:int}")]
        public async Task<ActionResult<IEnumerable<TipoCitaDto>>> GetPorEspecialidad(int idEspecialidad)
        {
            var tipos = await _tipoCitaService.ObtenerPorEspecialidadAsync(idEspecialidad);
            return Ok(tipos);
        }
    }
}
