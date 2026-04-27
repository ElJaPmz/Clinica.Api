using Clinica.Application.DTOs.ClinicaPerfil;
using Clinica.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicaController : ControllerBase
    {
        private readonly IClinicaService _clinicaService;

        public ClinicaController(IClinicaService clinicaService)
        {
            _clinicaService = clinicaService;
        }

        // POST: api/Clinica
        [HttpPost]
        [Authorize] // Solo el administrador puede crear la configuración inicial
        public async Task<ActionResult<ClinicaDto>> Post(ClinicaActualizarDto dto)
        {
            // Opcional: Validar si ya existe una clínica para no crear dos
            var existe = await _clinicaService.ObtenerPerfilAsync();
            if (existe != null)
                return BadRequest("La clínica ya está configurada. Use el método PUT para actualizar.");

            // Aquí usamos el mismo DTO de actualizar ya que tienen los mismos campos
            var resultado = await _clinicaService.CrearPerfilAsync(dto);

            if (!resultado)
                return BadRequest("No se pudo crear la configuración de la clínica.");

            return CreatedAtAction(nameof(Get), dto);
        }

        // GET: api/Clinica
        [HttpGet]
        [AllowAnonymous] // <--- PUBLICO: Sin candado en Swagger
        public async Task<ActionResult<ClinicaDto>> Get()
        {
            var perfil = await _clinicaService.ObtenerPerfilAsync();

            if (perfil == null)
                return NotFound("La información de la clínica no ha sido configurada.");

            return Ok(perfil);
        }

        // PUT: api/Clinica
        [HttpPut]
        [Authorize] // <--- PRIVADO: Requiere Token / Tiene candado
        public async Task<IActionResult> Put(ClinicaActualizarDto dto)
        {
            var resultado = await _clinicaService.ActualizarPerfilAsync(dto);

            if (!resultado)
                return BadRequest("No se pudo actualizar la información.");

            return NoContent();
        }
    }
}