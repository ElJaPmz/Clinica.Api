using AutoMapper;
using Clinica.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Clinica.Application.DTOs.Medico;
using Clinica.Application.Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


        // POST: api/Medicos
        [HttpPost]
        //[ProducesResponseType(typeof(MedicoDto), StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MedicoDto>> Crear([FromForm] MedicoCrearDto dto)
        {
            var creado = await _service.CrearAsync(dto);
                //.ConfigureAwait(false);
            return CreatedAtAction(nameof(GetMedico), new { id = creado.Id_Medico }, creado);

            ////var creado = await _service.CrearAsync(dto).ConfigureAwait(false);
            ////return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id_Medico }, creado);
            //var dto = new MedicoCrearDto
            //{
            //    Nombre = ,
            //    Apellido = "Perez",
            //    Id_Especialidad = 1,
            //    telefono = 123456789,
            //    Correo = "
        }

        // PUT: api/Medicos/5
        [HttpPut("{id}")]
        //[ProducesResponseType(typeof(MedicoDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MedicoDto>> Actualizar(int id, [FromForm] MedicoActualizarDto dto)
        {
            var actualizado = await _service.ActualizarAsync(id, dto).ConfigureAwait(false);
            return Ok(actualizado);
            //de esto no se como hacerlo :))))

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