using Clinica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Interface.Persistencia
{
    public interface IConsultorioRepository
    {
        Task<Consultorio?> ObtenerConsultorioPorIdAsync(int id);
        Task<IEnumerable<Consultorio>> ObtenerTodosAsync(int pagina, int tamanioPagina);
        Task<IEnumerable<Consultorio>> BuscarConsultorios( string valor,int pagina, int tamanioPagina);
        Task<int> ContarTodasAsync();
        Task<int> ContarConsultoriosPorBusquedaAsync(string valor);

        Task CrearAsync(Consultorio consultorio);
        Task ActualizarAsync(Consultorio consultorio);
        Task<int> EliminarAsync(int id);

    }
}
