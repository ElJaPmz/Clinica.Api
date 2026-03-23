using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Clinica.Application.DTOs.Paciente;
using Clinica.Application.Interface.Service;
using Clinica.Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;
        private readonly IMapper _mapper;

        public PacienteController(IPacienteService pacienteService, IMapper mapper)
        {
            _pacienteService = pacienteService;
            _mapper = mapper;
        }

        [HttpGet("ObtenerTodos")]
        public async Task<ActionResult<IEnumerable<PacienteDto>>> ObtenerTodos(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanioPagina = 10)
        {
            var pacientes = await _pacienteService.ObtenerTodosAsync(pagina, tamanioPagina);
            var total = await _pacienteService.ContarTodosAsync();

            return Ok(new RespuestaPaginada<PacienteDto>(pacientes, total, pagina, tamanioPagina));
        }

        [HttpGet("Buscar")]
        public async Task<ActionResult<IEnumerable<PacienteDto>>> Buscar(
            [FromQuery] string valor,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanioPagina = 10)
        {
            var pacientes = await _pacienteService.BuscarPacientes(valor, pagina, tamanioPagina);
            var total = await _pacienteService.ContarPacientesPorBusquedaAsync(valor);

            return Ok(new RespuestaPaginada<PacienteDto>(pacientes, total, pagina, tamanioPagina));
        }

        [HttpGet("{id:int}", Name = "GetPaciente")]
        public async Task<ActionResult<PacienteDto?>> GetPaciente(int id)
        {
            var paciente = await _pacienteService.ObtenerPacientePorIdAsync(id);
            return Ok(paciente);
        }

        [HttpPost("Crear")]
        public async Task<ActionResult<PacienteDto>> Crear([FromBody] PacienteCrearDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var created = await _pacienteService.CrearAsync(dto);

                return CreatedAtAction(
                    nameof(GetPaciente),
                    new { id = created.IdPaciente },
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
        public async Task<IActionResult> Actualizar(int id, [FromBody] PacienteActualizarDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _pacienteService.ActualizarAsync(id, dto);
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
            await _pacienteService.EliminarAsync(id);
            return NoContent();
        }
    }
}