using AutoMapper;
using Clinica.Application.DTOs.Paciente;
using Clinica.Application.Interface.Persistencia;
using Clinica.Application.Interface.Service;
using Clinica.Domain.Entities;

namespace Clinica.Application.Service
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;
        private readonly IMapper _mapper;

        public PacienteService(IPacienteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PacienteDto>> ObtenerTodosAsync(int pagina, int tamanioPagina)
        {
            var data = await _repository.ObtenerTodosAsync(pagina, tamanioPagina);
            return _mapper.Map<List<PacienteDto>>(data);
        }

        public async Task<int> ContarTodosAsync()
        {
            return await _repository.ContarTodosAsync();
        }

        public async Task<List<PacienteDto>> BuscarPacientes(string valor, int pagina, int tamanioPagina)
        {
            var data = await _repository.BuscarPacientes(valor, pagina, tamanioPagina);
            return _mapper.Map<List<PacienteDto>>(data);
        }

        public async Task<int> ContarPacientesPorBusquedaAsync(string valor)
        {
            return await _repository.ContarPacientesPorBusquedaAsync(valor);
        }

        public async Task<PacienteDto?> ObtenerPacientePorIdAsync(int id)
        {
            var data = await _repository.ObtenerPacientePorIdAsync(id);
            return _mapper.Map<PacienteDto>(data);
        }

        public async Task<PacienteDto> CrearAsync(PacienteCrearDto dto)
        {
            var entity = _mapper.Map<Paciente>(dto);
            await _repository.CrearAsync(entity);
            return _mapper.Map<PacienteDto>(entity);
        }

        public async Task ActualizarAsync(int id, PacienteActualizarDto dto)
        {
            var entity = _mapper.Map<Paciente>(dto);
            entity.IdPaciente = id;

            await _repository.ActualizarAsync(entity);
        }

        public async Task EliminarAsync(int id)
        {
            await _repository.EliminarAsync(id);
        }
    }
}