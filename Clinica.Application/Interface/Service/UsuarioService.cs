using AutoMapper;
using Clinica.Application.DTOs.Usuarios;
using Clinica.Application.Interface.Service;
using Clinica.Application.Mappings;
using Clinica.Domain.Entities;
using Clinica.Infrastructure.Interfaces.Persistencia;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerTodos()
        {
            var usuarios = await _usuarioRepository.GetAllUsuarios();
            // Aquí entra en juego tu MappingProfile para devolver UsuarioDto sin el hash
            return _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
        }

        public async Task<UsuarioDto?> ObtenerPorId(int id)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(id);
            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<bool> Actualizar(int id, UsuarioDto usuarioDto)
        {
            // 1. Buscar el usuario real en la DB
            var usuarioExistente = await _usuarioRepository.GetUsuarioById(id);

            if (usuarioExistente == null) return false;

            // 2. Actualizar las propiedades (sin tocar el PasswordHash)
            usuarioExistente.NombreUsuario = usuarioDto.NombreUsuario;
            usuarioExistente.Rol = usuarioDto.Rol;
            // Agrega aquí otros campos si tu UsuarioDto tiene más (ej. Correo, NombreReal)

            // 3. Notificar al repo y guardar
            _usuarioRepository.UpdateUsuario(usuarioExistente);
            return await _usuarioRepository.SaveChangesAsync();
        }

        public async Task<bool> Eliminar(int id)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(id);

            if (usuario == null) return false;

            _usuarioRepository.DeleteUsuario(usuario);
            return await _usuarioRepository.SaveChangesAsync();
        }
    }
}
