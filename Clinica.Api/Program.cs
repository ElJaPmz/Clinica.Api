using Clinica.Application.Interface.Persistencia;
using Clinica.Application.Interface.Service;
using Clinica.Application.Interfaces;
using Clinica.Application.Mappings;
using Clinica.Application.Service;
using Clinica.Application.Services;
using Clinica.Infrastructure.Data;
using Clinica.Infrastructure.Interfaces.Persistencia;
using Clinica.Infrastructure.Persistencia;
using Clinica.Infrastructure.Repository;
using Clinica.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Cargar variables de entorno
DotNetEnv.Env.Load();
builder.Configuration.AddEnvironmentVariables();

//leer las variables de conexión desde las variables de entorno
var host = Environment.GetEnvironmentVariable("HOST");
var port = Environment.GetEnvironmentVariable("PORT");
var database = Environment.GetEnvironmentVariable("DATABASE");
var user = Environment.GetEnvironmentVariable("USER");
var password = Environment.GetEnvironmentVariable("PASSWORD");

//Validar variables de entorno
var variablesFaltantes = new List<string>();
if (string.IsNullOrEmpty(host)) variablesFaltantes.Add("HOST");
if (string.IsNullOrEmpty(port)) variablesFaltantes.Add("PORT");
if (string.IsNullOrEmpty(database)) variablesFaltantes.Add("DATABASE");
if (string.IsNullOrEmpty(user)) variablesFaltantes.Add("USER");
if (string.IsNullOrEmpty(password)) variablesFaltantes.Add("PASSWORD");

if (variablesFaltantes.Any())
{
    throw new Exception($"Faltan variables de entorno: {string.Join("", variablesFaltantes)}");
}

// Construir la cadena de Conexión con postgrel
var connectionString =
    $"Host={host};" +
    $"Port={port};" +
    $"Database={database};" +
    $"Username={user};" +
    $"Password={password};";

//Registrar ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

//Registrar repositorios con sus interfaces
builder.Services.AddScoped<IConsultorioRepository, ConsultorioRepository>();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
builder.Services.AddScoped<IEspecialidadRepository, EspecialidadRepository>();
builder.Services.AddScoped<ICitaRepository, CitaRepository>();
builder.Services.AddScoped<IHistorialCitaRepository, HistorialCitaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IClinicaRepository, ClinicaRepository>();

//Registrar servicios con sus interfaces
builder.Services.AddScoped<IConsultorioService, ConsultorioService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<IEspecialidadService, EspecialidadService>();
builder.Services.AddScoped<ICitaService, CitaService>();
builder.Services.AddScoped<IHistorialCitaService, HistorialCitaService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IClinicaService, ClinicaService>();

// Configurar el esquema de autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Asegúrate de tener esto en appsettings.json
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

//Registrar AutoMapper
builder.Services.AddAutoMapper(cgf => { }, typeof(MappingProfile).Assembly);



// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    // 1. Aquí va TODA tu documentación detallada
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Clinica Management API",
        Description = @"
#### **Infraestructura integral para la gestión de servicios de salud.**

Esta API proporciona una plataforma centralizada para administrar el flujo operativo de una clínica, garantizando la integridad de los datos médicos, la agilidad en la atención y una arquitectura escalable.

---

####  Módulos del Sistema
* **🏥 Gestión de Pacientes:** Registro detallado, expedientes digitales y antecedentes clínicos.
* **📅 Control de Citas:** Programación inteligente de consultas y gestión de estados (Pendiente, Completada, Cancelada).
* **👨‍⚕️ Cuerpo Médico:** Administración de especialidades, disponibilidad horaria y asignación de consultorios.
* **💊 Historial y Diagnósticos:** Registro evolutivo de consultas, recetas médicas y seguimiento de tratamientos.

####  Características Técnicas
* **Seguridad:** Autenticación y autorización basada en **JWT (JSON Web Tokens)**.
* **Persistencia:** Gestión de datos robusta mediante **Entity Framework Core** y patrones de Repositorio.
* **Eficiencia:** Consultas optimizadas con soporte para **paginación y filtrado**.

---

####  Soporte Técnico
Para asistencia inmediata o reportar fallas en el sistema:
* **📞 Teléfono:** +505 8888-8888
* **📧 Correo:** soporte@clinica.com

---
"
    });

    // 2. Aquí configuramos el candado (Bearer Token)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT así: Bearer {tu_token}"
    });

    // 3. Esto vincula el token con tus endpoints

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
