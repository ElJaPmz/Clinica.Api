using Clinica.Application.DTOs.HistorialCita;
using Clinica.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinica.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialCitasController : ControllerBase
    {
        private readonly IHistorialCitaService _service;

        public HistorialCitasController(IHistorialCitaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistorialCitaDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("cita/{idCita}")]
        public async Task<ActionResult<IEnumerable<HistorialCitaDto>>> GetByCita(int idCita)
        {
            var result = await _service.GetByCitaIdAsync(idCita);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(HistorialCitaCrearDto crearDto)
        {
            var result = await _service.CreateAsync(crearDto);
            if (!result) return BadRequest("No se pudo registrar el historial.");

            return Ok(new { mensaje = "Historial registrado correctamente" });
        }

        [HttpPut]
        public async Task<ActionResult> Update(HistorialCitaActualizarDto actualizarDto)
        {
            var result = await _service.UpdateAsync(actualizarDto);
            if (!result) return BadRequest("No se pudo actualizar el historial.");

            return Ok(new { mensaje = "Historial actualizado correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound("Registro de historial no encontrado.");

            return Ok(new { mensaje = "Historial eliminado correctamente" });
        }
    }
}
