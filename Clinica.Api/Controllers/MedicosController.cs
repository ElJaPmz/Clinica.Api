using AutoMapper;
using Clinica.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clinica.Application.DTOs.Medico;
using Microsoft.AspNetCore.Http;
using Clinica.Application.Response;

namespace Clinica.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicosController : ControllerBase
    {
        private readonly IMedicoService _service;
        private readonly IMapper _mapper;

        public MedicosController(IMedicoService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/Medicos
        [HttpGet]
        //[ProducesResponseType(typeof(IEnumerable<MedicoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MedicoDto>>> ObtenerTodas(int pagina = 1, int tamanio = 10)
        {
            var medicos = await _service.ObtenerTodosAsync(pagina, tamanio).ConfigureAwait(false);
            var total = await _service.BuscarTodosAsync();

            return Ok(new RespuestaPaginada<MedicoDto>(medicos, total, pagina, tamanio));
        }

        // GET: api/Medicos/buscar?valor=juan&pagina=1&tamanio=10
        [HttpGet("buscar")]
        //[ProducesResponseType(typeof(IEnumerable<MedicoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MedicoDto>>> Buscar(string valor, int pagina = 1, int tamanio = 10)
        {
            var medicos = await _service.BuscarMedicosAsync(valor, pagina, tamanio).ConfigureAwait(false);
            var total = await _service.ContarMedicosPorBusquedaAsync(valor);

            return Ok(new RespuestaPaginada<MedicoDto>(medicos, total, pagina, tamanio));
        }


        // GET: api/Medicos/5
        [HttpGet("{id:int}", Name = "GetMedico")]
        //[ProducesResponseType(typeof(MedicoDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MedicoDto?>> GetMedico(int id)
        {
            var medico = await _service.ObtenerMedicoPorIdAsync(id);
                //.ConfigureAwait(false);
            return Ok(medico);
        }


        [HttpPost("Crear")]
        public async Task<ActionResult<MedicoDto>> Crear([FromBody] MedicoCrearDto dto)
        {
            // 1. Validación de seguridad del modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // 2. Llamada al servicio para crear el médico
                var creado = await _service.CrearAsync(dto);

                // 3. Retorno del recurso creado (201 Created)
                // Asegúrate de que el método GET se llame GetMedico
                return CreatedAtAction(nameof(GetMedico), new { id = creado.Id_Medico }, creado);
            }
            catch (ArgumentException ex)
            {
                // Errores de lógica de negocio (ej. especialidad no existe)
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Errores no controlados del servidor
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        // PUT: api/Medicos/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MedicoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MedicoDto>> Actualizar(int id, [FromBody] MedicoActualizarDto dto)
        {
            // 1. Validación de seguridad del modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // 2. Llamada al servicio para actualizar
                var actualizado = await _service.ActualizarAsync(id, dto);

                // 3. Retorno del objeto actualizado
                return Ok(actualizado);
            }
            catch (ArgumentException ex)
            {
                // Errores de validación (ej. ID no coincide o datos inválidos)
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                // Si el médico no existe en la base de datos
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Errores generales del servidor
                return StatusCode(500, $"Error interno al actualizar: {ex.Message}");
            }
        }

        // DELETE: api/Medicos/5
        [HttpDelete("{id}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _service.EliminarAsync(id);
                //.ConfigureAwait(false);
            return NoContent();
        }
    }
}