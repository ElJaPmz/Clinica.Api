using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Clinica.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaConsultorio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consultorios",
                columns: table => new
                {
                    Id_Consultorio = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumeroConsultorio = table.Column<int>(type: "integer", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultorios", x => x.Id_Consultorio);
                });

            migrationBuilder.CreateTable(
                name: "Especialidads",
                columns: table => new
                {
                    Id_Especialidad = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre_Especialidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidads", x => x.Id_Especialidad);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    IdPaciente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cedula = table.Column<string>(type: "character varying(19)", maxLength: 19, nullable: false),
                    TipoPaciente = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.IdPaciente);
                });

            migrationBuilder.CreateTable(
                name: "Medicos",
                columns: table => new
                {
                    Id_Medico = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Id_Especialidad = table.Column<int>(type: "integer", nullable: false),
                    telefono = table.Column<int>(type: "integer", maxLength: 10, nullable: false),
                    Correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicos", x => x.Id_Medico);
                    table.ForeignKey(
                        name: "FK_Medicos_Especialidads_Id_Especialidad",
                        column: x => x.Id_Especialidad,
                        principalTable: "Especialidads",
                        principalColumn: "Id_Especialidad",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Citas",
                columns: table => new
                {
                    IdCita = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPaciente = table.Column<int>(type: "integer", nullable: false),
                    IdMedico = table.Column<int>(type: "integer", nullable: false),
                    IdConsultorio = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "interval", nullable: false),
                    HoraFin = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EstadoCita = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    TipoCita = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas", x => x.IdCita);
                    table.ForeignKey(
                        name: "FK_Citas_Consultorios_IdConsultorio",
                        column: x => x.IdConsultorio,
                        principalTable: "Consultorios",
                        principalColumn: "Id_Consultorio",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citas_Medicos_IdMedico",
                        column: x => x.IdMedico,
                        principalTable: "Medicos",
                        principalColumn: "Id_Medico",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citas_Pacientes_IdPaciente",
                        column: x => x.IdPaciente,
                        principalTable: "Pacientes",
                        principalColumn: "IdPaciente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_IdConsultorio_Fecha_HoraInicio",
                table: "Citas",
                columns: new[] { "IdConsultorio", "Fecha", "HoraInicio" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Citas_IdMedico_Fecha_HoraInicio",
                table: "Citas",
                columns: new[] { "IdMedico", "Fecha", "HoraInicio" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Citas_IdPaciente",
                table: "Citas",
                column: "IdPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_Consultorios_NumeroConsultorio",
                table: "Consultorios",
                column: "NumeroConsultorio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Especialidads_Nombre_Especialidad",
                table: "Especialidads",
                column: "Nombre_Especialidad",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicos_Correo",
                table: "Medicos",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicos_Id_Especialidad",
                table: "Medicos",
                column: "Id_Especialidad");

            migrationBuilder.CreateIndex(
                name: "IX_Medicos_telefono",
                table: "Medicos",
                column: "telefono",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_Cedula",
                table: "Pacientes",
                column: "Cedula",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citas");

            migrationBuilder.DropTable(
                name: "Consultorios");

            migrationBuilder.DropTable(
                name: "Medicos");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Especialidads");
        }
    }
}
