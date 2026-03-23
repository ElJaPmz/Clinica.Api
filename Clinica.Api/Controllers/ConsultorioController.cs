using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Clinica.Application.DTOs.Consultorio;
using Clinica.Application.Interface.Service;
using Clinica.Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultorioController : ControllerBase
    {
        private readonly IConsultorioService _consultorioService;
        private readonly IMapper _mapper;

        public ConsultorioController(IConsultorioService consultorioService, IMapper mapper)
        {
            _consultorioService = consultorioService;
            _mapper = mapper;
        }

        [HttpGet("ObtenerTodos")]
        public async Task<ActionResult<IEnumerable<ConsultorioDto>>> ObtenerTodos([FromQuery] int pagina = 1, [FromQuery] int tamanioPagina = 10)
        {
            
            
                var consultorios = await _consultorioService.ObtenerTodosAsync(pagina, tamanioPagina);
                var total = await _consultorioService.ContarTodasAsync();
                return Ok(new RespuestaPaginada<ConsultorioDto>(consultorios,total,pagina,tamanioPagina));
          
        }

        [HttpGet("Buscar")]
        public async Task<ActionResult<IEnumerable<ConsultorioDto>>> Buscar([FromQuery] string valor, [FromQuery] int pagina = 1, [FromQuery] int tamanioPagina = 10)
        {
            var consultorios = await _consultorioService.BuscarConsultorios(valor,pagina, tamanioPagina);
            var total = await _consultorioService.ContarConsultoriosPorBusquedaAsync(valor);
            return Ok(new RespuestaPaginada<ConsultorioDto>(consultorios, total, pagina, tamanioPagina));
        }

        [HttpGet("{id:int}", Name = "GetConsultorio")]
        public async Task<ActionResult<ConsultorioDto?>>GetConsultorio(int id)
        {
           
                var consultorio = await _consultorioService.ObtenerConsultorioPorIdAsync(id);
                return Ok(consultorio);
            
        }

     

        [HttpPost("Crear")]
        public async Task<ActionResult<ConsultorioDto>> Crear([FromBody] ConsultorioCrearDto dto)
        {
            // 1. Validación de seguridad del modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var created = await _consultorioService.CrearAsync(dto);
                return CreatedAtAction(nameof(GetConsultorio), new { id = created.Id_Consultorio }, created);
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
        public async Task<IActionResult> Actualizar(int id, [FromBody] ConsultorioActualizarDto dto)
        {


            // 1. Validación de seguridad del modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _consultorioService.ActualizarAsync(id, dto);
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
            await _consultorioService.EliminarAsync(id);
            return NoContent();
            //try
            //{
            //    await _consultorioService.EliminarAsync(id);
            //    return NoContent();
            //}
            //catch (KeyNotFoundException)
            //{
            //    return NotFound();
            //}
            //catch (ArgumentException ex)
            //{
            //    return BadRequest(ex.Message);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, ex.Message);
            //}
        }
    }
}
