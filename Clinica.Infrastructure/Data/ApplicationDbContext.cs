using Clinica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Clinica.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Consultorio> Consultorios => Set<Consultorio>();
        public DbSet<Paciente> Pacientes => Set<Paciente>();
        public DbSet<Medico> Medicos => Set<Medico>();
        public DbSet<Especialidad> Especialidades => Set<Especialidad>();
        public DbSet<Cita> Citas => Set<Cita>();
        public DbSet<HistorialCita> HistorialCitas => Set<HistorialCita>();
        public DbSet<Recordatorio> Recordatorios => Set<Recordatorio>();
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ClinicaPerfil> Clinicas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // CONSULTORIOS
            builder.Entity<Consultorio>(entity =>
            {
                entity.HasKey(c => c.Id_Consultorio);
                entity.Property(c => c.Id_Consultorio).ValueGeneratedOnAdd();
                entity.Property(c => c.NumeroConsultorio).IsRequired();
                entity.Property(c => c.Descripcion).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Estado).IsRequired().HasMaxLength(50);
                entity.HasIndex(c => c.NumeroConsultorio).IsUnique();
            });

            // PACIENTES
            builder.Entity<Paciente>(entity =>
            {
                entity.HasKey(p => p.IdPaciente);
                entity.Property(p => p.IdPaciente).ValueGeneratedOnAdd();
                entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(p => p.FechaNacimiento).IsRequired();
                entity.Property(p => p.Telefono).IsRequired().HasMaxLength(20);
                entity.Property(p => p.Email).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Direccion).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Cedula).IsRequired().HasMaxLength(19);
                entity.Property(p => p.TipoPaciente).IsRequired().HasMaxLength(20);
                entity.HasIndex(p => p.Cedula).IsUnique();
            });

            // MEDICOS
            builder.Entity<Medico>(entity =>
            {
                entity.HasKey(m => m.Id_Medico);
                entity.Property(m => m.Id_Medico).ValueGeneratedOnAdd();
                entity.Property(m => m.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(m => m.Apellido).IsRequired().HasMaxLength(50);
                entity.Property(m => m.Id_Especialidad).IsRequired();
                entity.Property(m => m.telefono).IsRequired().HasMaxLength(10);
                entity.Property(m => m.Correo).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Estado).IsRequired().HasMaxLength(50);
                entity.HasIndex(m => m.Correo).IsUnique();
                entity.HasIndex(m => m.telefono).IsUnique();
                entity.HasOne<Especialidad>().WithMany().HasForeignKey(m => m.Id_Especialidad).OnDelete(DeleteBehavior.Restrict);
            });

            // ESPECIALIDADES
            builder.Entity<Especialidad>(entity =>
            {
                entity.HasKey(e => e.Id_Especialidad);
                entity.Property(e => e.Id_Especialidad).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre_Especialidad).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Nombre_Especialidad).IsUnique();
            });

            // CITAS
            builder.Entity<Cita>(entity =>
            {
                entity.HasKey(c => c.IdCita);
                entity.Property(c => c.IdCita).ValueGeneratedOnAdd();
                entity.Property(c => c.EstadoCita).IsRequired().HasMaxLength(30);
                entity.Property(c => c.TipoCita).IsRequired().HasMaxLength(30);

                entity.HasOne(c => c.Paciente).WithMany().HasForeignKey(c => c.IdPaciente).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.Medico).WithMany().HasForeignKey(c => c.IdMedico).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.Consultorio).WithMany().HasForeignKey(c => c.IdConsultorio).OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(c => new { c.IdMedico, c.Fecha, c.HoraInicio }).IsUnique();
                entity.HasIndex(c => new { c.IdConsultorio, c.Fecha, c.HoraInicio }).IsUnique();
            });

            // RECORDATORIOS
            builder.Entity<Recordatorio>(entity =>
            {
                entity.HasKey(r => r.IdRecordatorio);
                entity.Property(r => r.IdRecordatorio).ValueGeneratedOnAdd();
                entity.Property(r => r.TipoNotificacion).IsRequired().HasMaxLength(20);
                entity.Property(r => r.Estado).IsRequired().HasMaxLength(20);

                entity.HasOne<Cita>().WithMany().HasForeignKey(r => r.IdCita).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(r => new { r.IdCita, r.TipoNotificacion, r.FechaEnvio }).IsUnique();
            });

            // HISTORIAL DE CITAS
            builder.Entity<HistorialCita>(entity =>
            {
                entity.HasKey(h => h.Id_Historial);
                entity.Property(h => h.Id_Historial).ValueGeneratedOnAdd();
                entity.Property(h => h.Accion).IsRequired().HasMaxLength(20);
                entity.Property(h => h.Fecha_Hora).IsRequired().HasDefaultValueSql("NOW()"); 
                entity.Property(h => h.Comentario).HasMaxLength(500);

                entity.HasOne<Cita>().WithMany().HasForeignKey(h => h.Id_Cita).OnDelete(DeleteBehavior.Restrict);

                // Relación con Usuario
                entity.HasOne<Usuario>().WithMany().HasForeignKey(h => h.Id_Usuario).OnDelete(DeleteBehavior.Restrict);
            });

            // USUARIOS
            builder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id_Usuario);
                entity.Property(u => u.Id_Usuario).ValueGeneratedOnAdd();
                entity.Property(u => u.NombreUsuario).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Rol).IsRequired().HasMaxLength(30);

                entity.HasIndex(u => u.NombreUsuario).IsUnique();

                entity.HasOne<Medico>().WithMany().HasForeignKey(u => u.Id_Medico).OnDelete(DeleteBehavior.SetNull);
            });

            // CONFIGURACIÓN PARA INFORMACIÓN PÚBLICA DE LA CLÍNICA
            builder.Entity<ClinicaPerfil>(entity =>
            {
                // Define la Clave Primaria
                entity.HasKey(c => c.Id);

                // En PostgreSQL, ValueGeneratedOnAdd() mapeará a una columna tipo SERIAL o IDENTITY
                entity.Property(c => c.Id).ValueGeneratedOnAdd();

                entity.Property(c => c.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Descripcion)
                    .IsRequired()
                    .HasMaxLength(1000); // Más largo para que quepa la historia/misión

                entity.Property(c => c.Direccion)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(c => c.Telefono)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(c => c.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.ImagenPrincipal)
                    .IsRequired();
                // No le pongo MaxLength a la URL por si usas links muy largos de la nube
            });
        }
    }
}