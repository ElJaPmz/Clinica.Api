using System;
using System.Collections.Generic;
using System.Text;
using Clinica.Domain.Entities;

namespace Clinica.Application.Interface.Service
{
    public interface IJwtService
    {
        string CrearToken(Usuario usuario);
    }
}
