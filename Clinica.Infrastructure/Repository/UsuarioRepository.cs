using Clinica.Domain.Entities;
using Clinica.Infrastructure.Data;
using Clinica.Infrastructure.Interfaces.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetUsuarioByNombre(string nombreUsuario)
        {
            // Buscamos el usuario por su nombre de usuario de forma asíncrona
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
        }

        public async Task AddUsuario(Usuario usuario)
        {
            // Agregamos el nuevo usuario al contexto
            await _context.Usuarios.AddAsync(usuario);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Guardamos los cambios en la base de datos y retornamos true si se guardó algo
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> GetUsuarioById(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public void UpdateUsuario(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
        }

        public void DeleteUsuario(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
        }
    }
}
