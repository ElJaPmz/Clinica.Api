using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Clinica.Application.Interface;
using Microsoft.AspNetCore.Authorization;


namespace Clinica.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;
        public DashboardController(IDashboardService service) => _service = service;

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _service.ObtenerResumenEstrategico();
            return Ok(result);
        }
    }
}