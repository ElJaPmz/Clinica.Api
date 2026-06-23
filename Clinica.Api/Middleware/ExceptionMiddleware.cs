using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text.Json;

namespace Clinica.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _request;

        public ExceptionMiddleware(RequestDelegate request)
        {
            _request = request;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Ejecuta el siguiente middleware o el controlador
                await _request(context);
            }
            catch (Exception ex)
            {
                await ManejarExceptionAsync(context, ex);
            }
        }

        private Task ManejarExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            string mensaje;

            // Determinar el código de respuesta según el tipo de excepción
            switch (exception)
            {
                case ArgumentNullException:
                case ArgumentException:
                case InvalidOperationException:
                    statusCode = HttpStatusCode.BadRequest;             // 400
                    mensaje = exception.Message;
                    break;

                case SecurityTokenException securityTokenException:
                    statusCode = HttpStatusCode.BadRequest;             // 400
                    mensaje = $"Error de seguridad/token: '{securityTokenException.Message}'";
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;           // 401
                    mensaje = exception.Message;
                    break;

                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;               // 404
                    mensaje = exception.Message;
                    break;

                case DbUpdateException dbEx:                            // Especial para PostgreSQL y EF Core
                    statusCode = HttpStatusCode.BadRequest;
                    // Si Postgres lanza un error de llave duplicada o constraint, lo sacamos del InnerException
                    mensaje = dbEx.InnerException != null
                        ? dbEx.InnerException.Message
                        : dbEx.Message;
                    break;

                case AutoMapperMappingException autoMapperMappingEx:
                    statusCode = HttpStatusCode.InternalServerError;    // 500
                    mensaje = $"Error técnico de mapeo (AutoMapper): {autoMapperMappingEx.Message}";
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;    // 500
                    mensaje = "Error interno en el servidor clínico: " + exception.Message;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var respuesta = JsonSerializer.Serialize(new
            {
                status = (int)statusCode,
                detail = mensaje,
                type = exception.GetType().Name,
                instance = context.Request.Path // Útil para saber qué endpoint falló
            });

            return context.Response.WriteAsync(respuesta);
        }
    }
}