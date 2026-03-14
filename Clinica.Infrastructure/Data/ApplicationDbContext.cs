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
        }
    }
}