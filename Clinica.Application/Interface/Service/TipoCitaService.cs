using AutoMapper;
using Clinica.Application.DTOs.TipoCita;
using Clinica.Application.Interface.Persistencia;
using Clinica.Application.Interface.Service;

namespace Clinica.Application.Services
{
    public class TipoCitaService : ITipoCitaService
    {
        private readonly ITipoCitaRepository _repository;
        private readonly IMapper _mapper;

        public TipoCitaService(ITipoCitaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TipoCitaDto>> ObtenerTodosAsync()
        {
            var tipos = await _repository.ObtenerTodosAsync();
            return _mapper.Map<IEnumerable<TipoCitaDto>>(tipos);
        }

        public async Task<IEnumerable<TipoCitaDto>> ObtenerPorEspecialidadAsync(int idEspecialidad)
        {
            var tipos = await _repository.ObtenerPorEspecialidadAsync(idEspecialidad);
            return _mapper.Map<IEnumerable<TipoCitaDto>>(tipos);
        }
    }
}
