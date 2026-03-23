using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Clinica.Application.DTOs.Medico;
using Clinica.Application.Exceptions;
using Clinica.Application.Interface.Persistencia;
using Clinica.Application.Interface.Service;
using Clinica.Domain.Entities;

namespace Clinica.Application.Service
{
    public sealed class MedicoService: IMedicoService
    {
        private readonly IMedicoRepository _repository;
        private readonly IMapper _mapper;

        public MedicoService(IMedicoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MedicoDto?> ObtenerMedicoPorIdAsync(int id)
        {
            if (id <= 0)
            
                //throw new ArgumentException("El id debe ser mayor que cero.", nameof(id));
               throw new ArgumentException("El id debe ser mayor que cero.");

            var medico = await _repository.ObtenerMedicoPorIdAsync(id);
            if (medico == null)
                 throw new KeyNotFoundException($"No se encontró el registro con ID: {id}");
                //.ConfigureAwait(false);
                
            
            return _mapper.Map<MedicoDto>(medico);
            //var medico = await _repository.btenerMedicoPorIdAsync(id)
            //    //.ConfigureAwait(false);
            //return entidad is null ? null : _mapper.Map<MedicoDto>(entidad);
        }

        public async Task<IEnumerable<MedicoDto>> ObtenerTodosAsync(int pagina, int tamanioPagina)
        {
            var medicos = await _repository.ObtenerTodosAsync(pagina, tamanioPagina);
                //.ConfigureAwait(false);
            return _mapper.Map<IEnumerable<MedicoDto>>(medicos);
        }

        public async Task<IEnumerable<MedicoDto>> BuscarMedicosAsync(string valor, int pagina, int tamanioPagina)
        {
            var medicos = await _repository.BuscarMedicosAsync(valor, pagina, tamanioPagina);
                //.ConfigureAwait(false);
            return _mapper.Map<IEnumerable<MedicoDto>>(medicos);
        }

        public async Task<int> BuscarTodosAsync()
          => await _repository.BuscarTodosAsync();
        //.ConfigureAwait(false);


        public async Task<int> ContarMedicosPorBusquedaAsync(string valor)
            => await _repository.ContarMedicosPorBusquedaAsync(valor);
                //.ConfigureAwait(false);
        

        public async Task<MedicoDto> CrearAsync(MedicoCrearDto dto)
        {
            var medico = _mapper.Map<Medico>(dto);
            await _repository.CrearAsync(medico);
                //.ConfigureAwait(false);
            return _mapper.Map<MedicoDto>(medico);
        }

        public async Task<MedicoDto> ActualizarAsync(int id, MedicoActualizarDto dto)
        {
            var existente = await _repository.ObtenerMedicoPorIdAsync(id).ConfigureAwait(false);
            if (existente is null)
            {
                throw new InvalidOperationException($"Medico con id {id} no encontrado.");
            }

            _mapper.Map(dto, existente);

            await _repository.ActualizarAsync(existente);
                //.ConfigureAwait(false);
            return _mapper.Map<MedicoDto>(existente);
        }

        public async Task EliminarAsync(int id)
        {
            var medico = await _repository.ObtenerMedicoPorIdAsync(id)
                ?? throw new KeyNotFoundException("Registro no encontrado.");

            await _repository.EliminarAsync(id);
            //.ConfigureAwait(false);
            //var existente = await _repository.ObtenerMedicoPorIdAsync(id).ConfigureAwait(false);
            //if (existente is null)
            //{
            //    throw new NotFoundException($"Medico con id {id} no encontrado.");
            //}

            //await _repository.EliminarAsync(id).ConfigureAwait(false);
        }
    }
}