using Clinica.Api.Middleware;
using Clinica.Application.Interface;
using Clinica.Application.Interface.Persistencia;
using Clinica.Application.Interface.Service;
using Clinica.Application.Interfaces.Persistence;
using Clinica.Application.Interfaces.Service;
using Clinica.Application.Mappings;
using Clinica.Application.Service;
using Clinica.Application.Services;
using Clinica.Domain.Entities;
using Clinica.Infrastructure.Data;
using Clinica.Infrastructure.Persistence.Repositories;
using Clinica.Infrastructure.Persistencia;
//using Clinica.Infrastructure.Repositories;
using Clinica.Infrastructure.Repository;
using Clinica.Infrastructure.Service;
//using Clinica.Infrastructure.Services;
using Clinica.Persistence.Repositories;
//using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Cargar .env y variables de entorno
if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load();
}

builder.Configuration.AddEnvironmentVariables();

// Leer variables de conexión desde las variable de entorno (Usando DB_USER etc como el profe)
var user = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
var host = Environment.GetEnvironmentVariable("DB_HOST");
var database = Environment.GetEnvironmentVariable("DB_NAME");
var port = Environment.GetEnvironmentVariable("DB_PORT");
var key = Environment.GetEnvironmentVariable("JWT_KEY");
var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

// Validar variables de entorno para la conexión
var variablesFaltantes = new List<string>();
if (string.IsNullOrWhiteSpace(user)) variablesFaltantes.Add("DB_USER");
if (string.IsNullOrWhiteSpace(password)) variablesFaltantes.Add("DB_PASSWORD");
if (string.IsNullOrWhiteSpace(host)) variablesFaltantes.Add("DB_HOST");
if (string.IsNullOrWhiteSpace(database)) variablesFaltantes.Add("DB_NAME");
if (string.IsNullOrWhiteSpace(port)) variablesFaltantes.Add("DB_PORT");

if (variablesFaltantes.Any())
{
    throw new Exception($"Faltan variables de entorno: {string.Join(", ", variablesFaltantes)}");
}

// Contruir la cadena de conexión en formato PostgreSQL
var connectionString =
    $"Host={host};" +
    $"Port={port};" +
    $"Database={database};" +
    $"Username={user};" +
    $"Password={password};" +
    $"SSL Mode=Prefer;" +
    $"Trust Server Certificate=true;";
//1

// Registrar ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// Obtener credenciales de Cloudinary
var cloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME");
var apiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY");
var apiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET");

// Crear cuenta y cloudinary
//var account = new Account(cloudName, apiKey, apiSecret);
//var cloudinary = new Cloudinary(account) { Api = { Secure = true } };

// Registrar Cloudinary
//builder.Services.AddSingleton(cloudinary);

// Reglas de seguridad para la password y email (Identity)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Registrar repositorios con sus interfaces
builder.Services.AddScoped<IConsultorioRepository, ConsultorioRepository>();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
builder.Services.AddScoped<IEspecialidadRepository, EspecialidadRepository>();
builder.Services.AddScoped<ICitaRepository, CitaRepository>();
builder.Services.AddScoped<IHistorialCitaRepository, HistorialCitaRepository>();
builder.Services.AddScoped<IClinicaRepository, ClinicaRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITipoCitaRepository, TipoCitaRepository>();

// Registrar servicios con sus interfaces
builder.Services.AddScoped<IConsultorioService, ConsultorioService>();
//builder.Services.AddScoped<IImageStorageService, CloudinaryImageStorageService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<IEspecialidadService, EspecialidadService>();
builder.Services.AddScoped<ICitaService, CitaService>();
builder.Services.AddScoped<IHistorialCitaService, HistorialCitaService>();
builder.Services.AddScoped<IClinicaService, ClinicaService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITipoCitaService, TipoCitaService>();

// Configurar JWT Authentication
if (string.IsNullOrEmpty(key))
{
   throw new InvalidOperationException("La clave JWT no está configurada correctamente.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = true,
        ValidateAudience = true,
        RoleClaimType = ClaimTypes.Role,
        ValidIssuer = issuer,
        ValidAudience = audience
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                status = 401,
                detail = "No autenticado. El token es inválido o no fue enviado."
            }));
        },
        OnForbidden = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                status = 403,
                detail = "Acceso denegado. No tiene permisos para acceder a este recurso."
            }));
        }
    };
});

// Registrar AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

// Agregar controladores
builder.Services.AddControllers();

// Swagger / OpenAPI (ESTILO PROFE)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Clinica Management API",
        Version = "v1",
        Description = """
        #### **Infraestructura integral para la gestión de servicios de salud.**

        Esta API proporciona una plataforma centralizada para administrar el flujo operativo de una clínica.

        ---

        ### Módulos Principales:
        - **Gestión de Pacientes**: Expedientes digitales y antecedentes.
        - **Gestión de Citas**: Programación y estados de atención.
        - **Gestión Médica**: Especialidades y consultorios.
        - **Gestión de Usuarios**: Seguridad y roles con Identity.

        ---

        #### Acceso a Endpoints Protegidos
        Utilice el botón **Authorize** para ingresar su token JWT. Formato:
        ```
        Authorization: {token}
        ```
        """,
        Contact = new OpenApiContact { Name = "Soporte Técnico", Email = "soporte@clinica.com" }
    });

    // 1. Esquema de seguridad
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Token JWT requerido para acceder. Formato esperado: eyJhbGciOiJI..."
    });

    // 2. Requerimiento de seguridad (Sintaxis exacta del profe con document)
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference(referenceId: "Bearer", hostDocument: document),
            new List<string>()
        }
    });
});
//
// Configuración de CORS (Estilo StayInn)
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.WithOrigins("http://localhost:4200", "http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        else
        {
            policy.WithOrigins("https://clinicasave.netlify.app")// del deslpiegue netlify
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});
//2

builder.Services.AddOpenApi();

var app = builder.Build();

// Middleware para excepciones
app.UseMiddleware<ExceptionMiddleware>();

//
// Configuración Swagger UI
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Clinica API v1");
});

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger/index.html");
    return Task.CompletedTask;
});

// Redirección HTTP a HTTPS (solo en producción)
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("FrontendPolicy");

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

// Ejecución
if (app.Environment.IsDevelopment())
{
    // Aplicar migraciones automáticamente al iniciar
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();

        // Seeder: Crear tipos de cita por especialidad si no existen
        if (!db.TipoCitas.Any())
        {
            var especialidades = db.Especialidades.ToList();
            var tiposCitaParaSembrar = new List<TipoCita>();

            foreach (var esp in especialidades)
            {
                var nombre = esp.Nombre_Especialidad.Trim().ToLower();

                // Tipos genéricos que aplican a cualquier especialidad
                tiposCitaParaSembrar.AddRange(new[]
                {
                    new TipoCita { Nombre_TipoCita = $"Consulta de {esp.Nombre_Especialidad}", Id_Especialidad = esp.Id_Especialidad },
                    new TipoCita { Nombre_TipoCita = $"Control de {esp.Nombre_Especialidad}", Id_Especialidad = esp.Id_Especialidad },
                    new TipoCita { Nombre_TipoCita = $"Seguimiento especializado", Id_Especialidad = esp.Id_Especialidad },
                });
            }

            db.TipoCitas.AddRange(tiposCitaParaSembrar);
            db.SaveChanges();
            Console.WriteLine($"[Seeder] Se sembraron {tiposCitaParaSembrar.Count} tipos de cita.");
        }
    }

    app.Run();
}
else
{
    var apiPort = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    app.Run($"http://0.0.0.0:{apiPort}");
}