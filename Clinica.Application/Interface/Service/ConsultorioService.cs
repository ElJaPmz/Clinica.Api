using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Clinica.Application.DTOs.Consultorio;
using Clinica.Application.Interface.Persistencia;
using Clinica.Domain.Entities;

namespace Clinica.Application.Interface.Service
{
    public class ConsultorioService : IConsultorioService
    {
        private readonly IConsultorioRepository _consultorioRepository;
        private readonly IMapper _mapper;

        public ConsultorioService(IConsultorioRepository consultorioRepository, IMapper mapper)
        {
            _consultorioRepository = consultorioRepository;
            _mapper = mapper;
        }

        public async Task<ConsultorioDto?> ObtenerConsultorioPorIdAsync(int id)
        {
            if(id <=0)
            
                throw new ArgumentException("El id debe ser un número entero o mayor a cero.");
            var consultorios = await _consultorioRepository.ObtenerConsultorioPorIdAsync(id);
            if (consultorios is null)
                throw new KeyNotFoundException($"No se encontró el Consultorio con id {id}.");
            return _mapper.Map<ConsultorioDto>(consultorios);



            //var entidad = await _consultorioRepository.ObtenerConsultorioPorIdAsync(id);
            //return entidad is null ? null : _mapper.Map<ConsultorioDto>(entidad);
        }

        public async Task<IEnumerable<ConsultorioDto>> ObtenerTodosAsync(int pagina, int tamanioPagina)
        {
            var consultorios = await _consultorioRepository.ObtenerTodosAsync(pagina, tamanioPagina);
            return _mapper.Map<IEnumerable<ConsultorioDto>>(consultorios);
        }

        public async Task<IEnumerable<ConsultorioDto>> BuscarConsultorios(string valor, int pagina, int tamanioPagina)
        {
            var consultorios = await _consultorioRepository.BuscarConsultorios(valor, pagina, tamanioPagina);
            return _mapper.Map<IEnumerable<ConsultorioDto>>(consultorios);
        }

        public async Task<int> ContarTodasAsync()
        {
            return await _consultorioRepository.ContarTodasAsync();
        }

        public async Task<int> ContarConsultoriosPorBusquedaAsync(string valor)
        {
            return await _consultorioRepository.ContarConsultoriosPorBusquedaAsync(valor);
        }

        public async Task<ConsultorioDto> CrearAsync(ConsultorioCrearDto dto)
        {
            // 1. Validación manual (Defensiva)
            if (dto == null)
                throw new ArgumentException("Los datos del consultorio no pueden ser nulos.");

            // 2. Mapeo a la entidad de dominio
            var consultorio = _mapper.Map<Consultorio>(dto);

            // 3. Persistencia en la DB
            await _consultorioRepository.CrearAsync(consultorio);

            // 4. Retornar el DTO con el ID ya generado por Postgres
            return _mapper.Map<ConsultorioDto>(consultorio);
        }

        public async Task<ConsultorioDto> ActualizarAsync(int id, ConsultorioActualizarDto dto)
        {
            if (id <= 0)
                throw new ArgumentException("El id debe ser mayor a cero.");
            var consultorio = await _consultorioRepository.ObtenerConsultorioPorIdAsync(id);
            if (consultorio is null)
            {
                throw new KeyNotFoundException($"Consultorio con id {id} no encontrado.");
            }

            _mapper.Map(dto, consultorio);
            await _consultorioRepository.ActualizarAsync(consultorio);
            return _mapper.Map<ConsultorioDto>(consultorio);
        }

        public async Task EliminarAsync(int id)
        {
            // Validamos que el ID tenga sentido
            if (id <= 0)
                throw new ArgumentException("El id debe ser mayor a cero.");

            // Le pedimos al repositorio que borre el registro con ese ID
            // El repositorio nos dirá cuántas filas (registros) borró
            int registrosBorrados = await _consultorioRepository.EliminarAsync(id);

            // Si el número es 0, significa que el ID no existía en la tabla
            if (registrosBorrados == 0)
            {
                throw new KeyNotFoundException($"No se encontró un consultorio con el ID {id}.");
            }
        }
    }
}
