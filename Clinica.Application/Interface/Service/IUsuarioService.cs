using Clinica.Application.DTOs;
using Clinica.Application.DTOs.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Interface.Service
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDto>> ObtenerTodos();
        Task<UsuarioDto?> ObtenerPorId(int id);
        Task<bool> Actualizar(int id, UsuarioDto usuarioDto); // <--- NUEVO
        Task<bool> Eliminar(int id);
    }
}
