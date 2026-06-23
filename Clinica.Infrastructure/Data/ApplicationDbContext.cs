using Clinica.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
        public DbSet<ClinicaPerfil> Clinicas => Set<ClinicaPerfil>();
        public DbSet<TipoCita> TipoCitas => Set<TipoCita>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // 1. Configuración base de Identity
            base.OnModelCreating(builder);

            // 2. Configuración de ApplicationUser
            builder.Entity<ApplicationUser>(static entity =>
            {
                entity.Property(u => u.NombreCompleto).IsRequired().HasMaxLength(150);
                entity.Property(u => u.Activo).HasDefaultValue(true);

                entity.HasOne<Medico>()
                    .WithMany()
                    .HasForeignKey(u => u.Id_Medico)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // 3. Consultorio
            builder.Entity<Consultorio>(static entity =>
            {
                entity.HasKey(c => c.Id_Consultorio);
                entity.Property(c => c.Id_Consultorio).ValueGeneratedOnAdd();
                entity.Property(c => c.NumeroConsultorio).IsRequired();
                entity.Property(c => c.Descripcion).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Estado).IsRequired().HasMaxLength(50);
                entity.HasIndex(c => c.NumeroConsultorio).IsUnique();
            });

            // 4. Paciente
            builder.Entity<Paciente>(static entity =>
            {
                entity.HasKey(p => p.IdPaciente);
                entity.Property(p => p.IdPaciente).ValueGeneratedOnAdd();
                entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Cedula).IsRequired().HasMaxLength(19);
                entity.HasIndex(p => p.Cedula).IsUnique();

                entity.ToTable(p =>
                {
                    p.HasCheckConstraint("CK_Paciente_Email", "\"Email\" LIKE '%_@__%.__%'");
                });
            });

            // 5. Médico
            builder.Entity<Medico>(static entity =>
            {
                entity.HasKey(m => m.Id_Medico);
                entity.Property(m => m.Id_Medico).ValueGeneratedOnAdd();
                entity.Property(m => m.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(m => m.Correo).IsRequired().HasMaxLength(100);
                entity.HasIndex(m => m.Correo).IsUnique();

                entity.HasOne(m => m.Especialidad)
                    .WithMany()
                    .HasForeignKey(m => m.Id_Especialidad)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });

            // 5b. TipoCita
            builder.Entity<TipoCita>(entity =>
            {
                entity.HasKey(t => t.Id_TipoCita);
                entity.Property(t => t.Id_TipoCita).ValueGeneratedOnAdd();
                entity.Property(t => t.Nombre_TipoCita).IsRequired().HasMaxLength(100);

                entity.HasOne(t => t.Especialidad)
                    .WithMany()
                    .HasForeignKey(t => t.Id_Especialidad)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });

            // 6. Cita
            builder.Entity<Cita>(static entity =>
            {
                entity.HasKey(c => c.IdCita);
                entity.Property(c => c.IdCita).ValueGeneratedOnAdd();

                // Configuración de tipos para Postgres
                entity.Property(c => c.Fecha).IsRequired().HasColumnType("date");
                entity.Property(c => c.HoraInicio).IsRequired();
                entity.Property(c => c.HoraFin).IsRequired();

                entity.Property(c => c.EstadoCita).IsRequired().HasMaxLength(30);
                entity.Property(c => c.TipoCita).IsRequired().HasMaxLength(30);

                // Relación con Paciente
                entity.HasOne(c => c.Paciente)
                    .WithMany(p => p.Citas)
                    .HasForeignKey(c => c.IdPaciente)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relación con Médico (Usando IdMedico de la clase Cita)
                entity.HasOne(c => c.Medico)
                    .WithMany(m => m.Citas)
                    .HasForeignKey(c => c.IdMedico)
                    .OnDelete(DeleteBehavior.Restrict);

                // --- ESTA ES LA PARTE QUE FALTABA ---
                entity.HasOne(c => c.Consultorio)
                    .WithMany() // O .WithMany(con => con.Citas) si agregas la lista en Consultorio
                    .HasForeignKey(c => c.IdConsultorio)
                    .OnDelete(DeleteBehavior.Restrict);
                // ------------------------------------

                entity.ToTable(c =>
                {
                    c.HasCheckConstraint(
                        "CK_Cita_Estado",
                        "\"EstadoCita\" IN ('Pendiente', 'Confirmada', 'Atendida', 'Cancelada', 'Ausente')"
                    );
                });
            });

            // 7. HistorialCita
            builder.Entity<HistorialCita>(static entity =>
            {
                entity.HasKey(h => h.Id_Historial);
                entity.Property(h => h.Accion).IsRequired().HasMaxLength(50);
                entity.Property(h => h.Fecha_Hora).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne<Cita>()
                    .WithMany()
                    .HasForeignKey(h => h.Id_Cita)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 8. Clínica Perfil
            builder.Entity<ClinicaPerfil>(static entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
                entity.Property(c => c.ImagenPrincipal).IsRequired().HasMaxLength(500);
            });

            // 9. Especialidad (Corregido con tus nombres exactos)
            builder.Entity<Especialidad>(static entity =>
            {
                entity.HasKey(e => e.Id_Especialidad); // PK manual
                entity.Property(e => e.Id_Especialidad).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre_Especialidad).IsRequired().HasMaxLength(100);
            });

            // 10. Recordatorio (Corregido con tus nombres exactos)
            builder.Entity<Recordatorio>(static entity =>
            {
                entity.HasKey(r => r.IdRecordatorio); // PK manual
                entity.Property(r => r.IdRecordatorio).ValueGeneratedOnAdd();

                entity.Property(r => r.TipoNotificacion).IsRequired().HasMaxLength(50);
                entity.Property(r => r.Estado).IsRequired().HasMaxLength(50);

                // Para Postgres usamos el tipo timestamp
                entity.Property(r => r.FechaEnvio).IsRequired().HasColumnType("timestamp without time zone");

                // Relación con Cita
                entity.HasOne<Cita>()
                    .WithMany()
                    .HasForeignKey(r => r.IdCita)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}