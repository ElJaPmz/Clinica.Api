using Clinica.Application.DTOs.Cita;
using Clinica.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly ICitaService _citaService;

        public CitasController(ICitaService citaService)
        {
            _citaService = citaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CitaDto>>> Get()
        {
            var citas = await _citaService.ObtenerTodasAsync();
            return Ok(citas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CitaDto>> Get(int id)
        {
            var cita = await _citaService.ObtenerPorIdAsync(id);
            if (cita == null) return NotFound($"No se encontró la cita con ID {id}");

            return Ok(cita);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CitaCrearDto citaCrearDto)
        {
            try
            {
                var nuevaCita = await _citaService.CrearAsync(citaCrearDto);

                // Cambiamos GetById por Get, que es como se llama tu método en la línea 21
                return CreatedAtAction(nameof(Get), new { id = nuevaCita.IdCita }, nuevaCita);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CitaActualizarDto citaActualizarDto)
        {
            try
            {
                // 1. Ejecutamos la actualización (Aquí es donde saltará la excepción si hay choque)
                var resultado = await _citaService.ActualizarAsync(id, citaActualizarDto);

                if (!resultado)
                    return NotFound($"No se pudo actualizar. La cita con ID {id} no existe.");

                // 2. Recuperamos la cita actualizada con sus Includes
                var citaActualizada = await _citaService.ObtenerPorIdAsync(id);

                // 3. Devolvemos la data completa
                return Ok(citaActualizada);
            }
            catch (Exception ex)
            {
                // Esto captura el "throw new Exception" del Service y lo devuelve limpio
                // En Swagger verás un 400 Bad Request con el mensaje exacto
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _citaService.EliminarAsync(id);

            if (!resultado) return NotFound($"No se pudo eliminar. La cita con ID {id} no existe.");

            return NoContent();
        }

        [HttpGet("medico/{idMedico}")]
        public async Task<ActionResult<IEnumerable<CitaDto>>> GetPorMedico(int idMedico)
        {
            var citas = await _citaService.ObtenerCitasPorMedicoAsync(idMedico);

            // Si la lista está vacía, igual devolvemos un Ok con lista vacía []
            return Ok(citas);
        }

        [HttpGet("paciente/{idPaciente}")]
        public async Task<ActionResult<IEnumerable<CitaDto>>> GetPorPaciente(int idPaciente)
        {
            var citas = await _citaService.ObtenerCitasPorPacienteAsync(idPaciente);

            return Ok(citas);
        }
    }
}
