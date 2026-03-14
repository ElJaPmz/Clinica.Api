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
        }
       
    }
}
