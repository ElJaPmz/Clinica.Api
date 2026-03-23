using Clinica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Interface.Persistencia
{
    public interface IMedicoRepository
    {
        //Metoedos de consulta
        Task<Medico?> ObtenerMedicoPorIdAsync(int id);
        Task<IEnumerable<Medico>> ObtenerTodosAsync(int pagina, int tamanioPagina);
        Task<IEnumerable<Medico>> BuscarMedicosAsync(string valor, int pagina, int tamanioPagina);
        Task<int> BuscarTodosAsync();
        Task<int> ContarMedicosPorBusquedaAsync(string valor);

        //Metodos de modificacion
        Task CrearAsync(Medico medico);
        Task ActualizarAsync(Medico medico);
        Task<int> EliminarAsync(int id);

    }
}
