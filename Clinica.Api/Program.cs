using Clinica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
