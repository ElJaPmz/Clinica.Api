using AutoMapper;
using Clinica.Application.DTOs.Consultorio;
using Clinica.Application.DTOs.Medico;
using Clinica.Application.DTOs.Paciente;
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
        }
    }
}
