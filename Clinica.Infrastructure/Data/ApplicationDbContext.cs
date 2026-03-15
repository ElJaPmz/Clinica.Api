using Clinica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Consultorios
            builder.Entity<Consultorio>(static entity =>
            {
                entity.HasKey(c => c.Id_Consultorio);

                entity.Property(c => c.Id_Consultorio)
                .ValueGeneratedOnAdd();

                entity.Property(c => c.NumeroConsultorio)
                .IsRequired();

                entity.Property(c => c.Descripcion)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(c => c.Estado)
                .IsRequired()
                .HasMaxLength(50);

                //Restricción de número de consultorio para no repetir el mismo número
                entity.HasIndex(c => c.NumeroConsultorio)
                .IsUnique();
            });

            // Pacientes
            builder.Entity<Paciente>(static entity =>
            {
                entity.HasKey(p => p.IdPaciente);

                entity.Property(p => p.IdPaciente)
                .ValueGeneratedOnAdd();

                entity.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(p => p.Apellido)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(p => p.FechaNacimiento)
                .IsRequired();

                entity.Property(p => p.Telefono)
                .IsRequired()
                .HasMaxLength(20);

                entity.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(p => p.Direccion)
                .IsRequired()
                .HasMaxLength(200);

                entity.Property(p => p.Cedula)
                .IsRequired()
                .HasMaxLength(19);

                entity.Property(p => p.TipoPaciente)
                .IsRequired()
                .HasMaxLength(20);

                // Restricción para evitar documentos duplicados
                entity.HasIndex(p => p.Cedula)
                .IsUnique();
            });


            // cuando migre se cree la tabla Medicos con las propiedades de la clase Medico
            // Medicos
            builder.Entity<Medico>(static entity =>
            {
                entity.HasKey(m => m.Id_Medico);

                entity.Property(m => m.Id_Medico)
                .ValueGeneratedOnAdd();

                entity.Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(15);

                entity.Property(m => m.Apellido)
                .IsRequired()
                .HasMaxLength(15);

                entity.Property(m => m.Id_Especialidad)
                .IsRequired();

                entity.Property(m => m.telefono)
                .IsRequired()
                .HasMaxLength(10);

                entity.Property(m => m.Correo)
                .IsRequired()
                .HasMaxLength(30);

                entity.Property(m => m.Estado)
                .IsRequired()
                .HasMaxLength(50);


                //Restricción de número de consultorio para no repetir el mismo número
                entity.HasIndex(m => m.Correo)
                .IsUnique();

                entity.HasIndex(m => m.telefono)
                .IsUnique();

                //Relacion entre Medico y Especialidad---(necesito hacer la relacion de ambos)
                //entity.HasOne(m => m.Id_Especialidad);
                //.WithMany(e => e.Id_Especialidad)


            });
        }
       
    }
}