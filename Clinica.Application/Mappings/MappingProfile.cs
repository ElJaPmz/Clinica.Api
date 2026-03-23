using AutoMapper;
using Clinica.Application.DTOs.Consultorio;
using Clinica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            #region Consultorios
            CreateMap<Consultorio, ConsultorioDto>();
            CreateMap<ConsultorioCrearDto, Consultorio>();
            CreateMap<ConsultorioActualizarDto, Consultorio>()
                .ForMember(dest => dest.Id_Consultorio, opt => opt.Ignore());
            #endregion
        }
    }
}
