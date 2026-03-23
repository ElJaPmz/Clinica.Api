using AutoMapper;
using Clinica.Application.DTOs.Medico;
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
            #region Medicos
            CreateMap<Medico, MedicoDto>();

            CreateMap<MedicoDto, Medico>();


            CreateMap<Medico, MedicoActualizarDto>();

            CreateMap<MedicoActualizarDto, Medico>()
                .ForMember(dest => dest.Id_Medico, opt => opt.Ignore());


            #endregion
        }
    }
}
