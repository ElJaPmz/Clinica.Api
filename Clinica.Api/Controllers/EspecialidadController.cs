using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clinica.Application.DTOs.Especialidad;
using Clinica.Application.Interface.Service;
using Clinica.Application.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadController : ControllerBase
    {
        private readonly IEspecialidadService _especialidadService;

        public EspecialidadController(IEspecialidadService especialidadService)
        {
            _especialidadService = especialidadService;
        }

        [HttpGet("ObtenerTodos")]
        public async Task<ActionResult<IEnumerable<EspecialidadDto>>> ObtenerTodos(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanioPagina = 10)
        {
            var especialidades = await _especialidadService.ObtenerTodosAsync(pagina, tamanioPagina);
            var total = await _especialidadService.ContarTodasAsync();

            return Ok(new RespuestaPaginada<EspecialidadDto>(especialidades, total, pagina, tamanioPagina));
        }

        [HttpGet("Buscar")]
        public async Task<ActionResult<IEnumerable<EspecialidadDto>>> Buscar(
            [FromQuery] string valor,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanioPagina = 10)
        {
            var especialidades = await _especialidadService.BuscarAsync(valor, pagina, tamanioPagina);
            var total = await _especialidadService.ContarPorBusquedaAsync(valor);

            return Ok(new RespuestaPaginada<EspecialidadDto>(especialidades, total, pagina, tamanioPagina));
        }

        [HttpGet("{id:int}", Name = "GetEspecialidad")]
        public async Task<ActionResult<EspecialidadDto?>> GetEspecialidad(int id)
        {
            var especialidad = await _especialidadService.ObtenerPorIdAsync(id);
            return Ok(especialidad);
        }

        [HttpPost("Crear")]
        public async Task<ActionResult<EspecialidadDto>> Crear([FromBody] EspecialidadCrearDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _especialidadService.CrearAsync(dto);

                return CreatedAtAction(
                    nameof(GetEspecialidad),
                    new { id = created.Id_Especialidad },
                    created
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] EspecialidadActualizarDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _especialidadService.ActualizarAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _especialidadService.EliminarAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}