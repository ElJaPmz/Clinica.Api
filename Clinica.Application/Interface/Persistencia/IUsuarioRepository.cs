using System;
using System.Collections.Generic;
using System.Text;
using Clinica.Domain.Entities;

namespace Clinica.Infrastructure.Interfaces.Persistencia
{
    public interface IUsuarioRepository
    {
        // Para buscar si un nombre de usuario ya existe antes de registrarlo
        Task<Usuario?> GetUsuarioByNombre(string nombreUsuario);

        // Para guardar el nuevo usuario en la tabla
        Task AddUsuario(Usuario usuario);

        // Para confirmar los cambios en la DB
        Task<bool> SaveChangesAsync();

        // NUEVOS MÉTODOS
        Task<IEnumerable<Usuario>> GetAllUsuarios(); // Para el listado
        Task<Usuario?> GetUsuarioById(int id);       // Para buscar por ID antes de editar/borrar
        void UpdateUsuario(Usuario usuario);         // Para actualizar datos
        void DeleteUsuario(Usuario usuario);         // Para eliminar
    }
}
