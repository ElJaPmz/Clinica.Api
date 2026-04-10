using AutoMapper;
using Clinica.Application.DTOs.Cita;
using Clinica.Application.Interface.Persistencia;
using Clinica.Application.Interface.Service;
using Clinica.Domain.Entities;

namespace Clinica.Application.Service
{
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IMapper _mapper;

        public CitaService(ICitaRepository citaRepository, IMapper mapper)
        {
            _citaRepository = citaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CitaDto>> ObtenerTodasAsync()
        {
            var citas = await _citaRepository.ObtenerTodasAsync();
            return _mapper.Map<IEnumerable<CitaDto>>(citas);
        }

        public async Task<CitaDto?> ObtenerPorIdAsync(int id)
        {
            var cita = await _citaRepository.ObtenerPorIdAsync(id);
            return _mapper.Map<CitaDto>(cita);
        }

        public async Task<CitaDto> CrearAsync(CitaCrearDto dto)
        {
            // 1. VALIDACIÓN: ¿Médico ocupado?
            var citasMedico = await _citaRepository.ObtenerCitasPorMedicoAsync(dto.IdMedico);
            bool choqueMedico = citasMedico.Any(c =>
                c.Fecha.Date == dto.Fecha.Date &&
                (dto.HoraInicio < c.HoraFin && dto.HoraFin > c.HoraInicio)
            );

            if (choqueMedico)
                throw new Exception("El médico ya tiene otra cita programada en ese horario.");

            // 2. VALIDACIÓN: ¿Consultorio ocupado?
            var todasLasCitas = await _citaRepository.ObtenerTodasAsync();
            bool choqueConsultorio = todasLasCitas.Any(c =>
                c.IdConsultorio == dto.IdConsultorio &&
                c.Fecha.Date == dto.Fecha.Date &&
                (dto.HoraInicio < c.HoraFin && dto.HoraFin > c.HoraInicio)
            );

            if (choqueConsultorio)
                throw new Exception("El consultorio ya está reservado por otro médico en ese horario.");

            // 3. Mapeamos el DTO a la Entidad para guardar
            var citaEntidad = _mapper.Map<Cita>(dto);

            // 4. Guardamos en la base de datos
            await _citaRepository.CrearAsync(citaEntidad);

            // 5. RECARGAMOS: Buscamos la cita con sus .Include() para traer los nombres
            var citaCompleta = await _citaRepository.ObtenerPorIdAsync(citaEntidad.IdCita);

            // 6. Retornamos el DTO con la información completa (nombres incluidos)
            return _mapper.Map<CitaDto>(citaCompleta);
        }

        public async Task<bool> ActualizarAsync(int id, CitaActualizarDto dto)
        {
            // 1. Verificar existencia
            var citaExistente = await _citaRepository.ObtenerPorIdAsync(id);
            if (citaExistente == null) return false;

            // 2. VALIDACIÓN: ¿Médico ocupado?
            var citasMedico = await _citaRepository.ObtenerCitasPorMedicoAsync(dto.IdMedico);
            bool choqueMedico = citasMedico.Any(c =>
                c.IdCita != id &&
                c.Fecha.Date == dto.Fecha.Date &&
                (dto.HoraInicio < c.HoraFin && dto.HoraFin > c.HoraInicio)
            );

            if (choqueMedico)
                throw new Exception("El médico ya tiene otra cita programada en ese horario.");

            // 3. VALIDACIÓN: ¿Consultorio ocupado?
            // Primero necesitamos obtener las citas de ese consultorio
            var todasLasCitas = await _citaRepository.ObtenerTodasAsync();

            bool choqueConsultorio = todasLasCitas.Any(c =>
                c.IdCita != id &&
                c.IdConsultorio == dto.IdConsultorio &&
                c.Fecha.Date == dto.Fecha.Date &&
                (dto.HoraInicio < c.HoraFin && dto.HoraFin > c.HoraInicio)
            );

            if (choqueConsultorio)
                throw new Exception("El consultorio ya está reservado por otro médico en ese horario.");

            // 4. Si pasa ambas, guardamos
            _mapper.Map(dto, citaExistente);
            await _citaRepository.ActualizarAsync(citaExistente);
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var citaExistente = await _citaRepository.ObtenerPorIdAsync(id);
            if (citaExistente == null) return false;

            await _citaRepository.EliminarAsync(id);
            return true;
        }

        public async Task<IEnumerable<CitaDto>> ObtenerCitasPorMedicoAsync(int idMedico)
        {
            var citas = await _citaRepository.ObtenerCitasPorMedicoAsync(idMedico);
            return _mapper.Map<IEnumerable<CitaDto>>(citas);
        }

        public async Task<IEnumerable<CitaDto>> ObtenerCitasPorPacienteAsync(int idPaciente)
        {
            var citas = await _citaRepository.ObtenerCitasPorPacienteAsync(idPaciente);
            return _mapper.Map<IEnumerable<CitaDto>>(citas);
        }
    }
}