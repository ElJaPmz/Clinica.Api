using AutoMapper;
using Clinica.Application.DTOs.Cita;
using Clinica.Application.DTOs.ClinicaPerfil;
using Clinica.Application.DTOs.Consultorio;
using Clinica.Application.DTOs.Especialidad;
using Clinica.Application.DTOs.HistorialCita;
using Clinica.Application.DTOs.Medico;
using Clinica.Application.DTOs.Paciente;
using Clinica.Application.DTOs.TipoCita;
using Clinica.Application.DTOs.Usuarios;
using Clinica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Consultorios
            CreateMap<Consultorio, ConsultorioDto>();
            CreateMap<ConsultorioCrearDto, Consultorio>();
            CreateMap<ConsultorioActualizarDto, Consultorio>()
                .ForMember(dest => dest.Id_Consultorio, opt => opt.Ignore());
            #endregion

            #region Pacientes
            CreateMap<Paciente, PacienteDto>();

            CreateMap<PacienteCrearDto, Paciente>();

            CreateMap<PacienteActualizarDto, Paciente>()
                .ForMember(dest => dest.IdPaciente, opt => opt.Ignore());
            #endregion

            #region Medicos
            CreateMap<Medico, MedicoDto>();
            CreateMap<MedicoCrearDto, Medico>();
            CreateMap<MedicoActualizarDto, Medico>()
                .ForMember(dest => dest.Id_Medico, opt => opt.Ignore());
            #endregion

            #region Citas
            // Este es el que envía la información a Swagger 
            CreateMap<Cita, CitaDto>()
                .ForMember(dest => dest.NombreCompletoPaciente,
                           opt => opt.MapFrom(src => src.Paciente != null
                           ? $"{src.Paciente.Nombre} {src.Paciente.Apellido}"
                           : "Paciente no encontrado"))

                .ForMember(dest => dest.NombreCompletoMedico,
                           opt => opt.MapFrom(src => src.Medico != null
                           ? $"{src.Medico.Nombre} {src.Medico.Apellido}"
                           : "Médico no encontrado"))

                .ForMember(dest => dest.InfoConsultorio,
                           opt => opt.MapFrom(src => src.Consultorio != null
                           ? $"#{src.Consultorio.NumeroConsultorio} - {src.Consultorio.Descripcion}"
                           : "Sin consultorio"));

            // Este es el que recibe los datos para crear
            CreateMap<CitaCrearDto, Cita>()
                .ForMember(dest => dest.EstadoCita, opt => opt.MapFrom(src => "Pendiente"));

            // Este es el que recibe los datos para actualizar
            CreateMap<CitaActualizarDto, Cita>()
                .ForMember(dest => dest.IdCita, opt => opt.Ignore());
            #endregion

            #region Especialidad
            CreateMap<Especialidad, EspecialidadDto>();
            CreateMap<EspecialidadCrearDto, Especialidad>();
            CreateMap<EspecialidadActualizarDto, Especialidad>()
                .ForMember(dest => dest.Id_Especialidad, opt => opt.Ignore());
            #endregion

            #region TipoCita
            CreateMap<TipoCita, TipoCitaDto>();
            #endregion

            #region HistorialCitas
            CreateMap<HistorialCita, HistorialCitaDto>();

            CreateMap<HistorialCitaCrearDto, HistorialCita>();

            CreateMap<HistorialCitaActualizarDto, HistorialCita>()
                .ForMember(dest => dest.Id_Historial, opt => opt.Ignore());
            #endregion

            #region Usuarios
            CreateMap<ApplicationUser, UsuarioDto>();

            CreateMap<UsuarioRegistroDto, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            #endregion


            #region ClinicaPerfil
            // Mapeo para mostrar la información en el GET
            CreateMap<ClinicaPerfil, ClinicaDto>();

            // Mapeo para actualizar la información desde el PUT
            CreateMap<ClinicaActualizarDto, ClinicaPerfil>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // El ID no se actualiza desde el DTO
            #endregion
        }
    }
}
