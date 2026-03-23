using Clinica.Domain.Entities;

public interface IPacienteRepository
{
    Task<Paciente?> ObtenerPacientePorIdAsync(int id);

    Task<IEnumerable<Paciente>> ObtenerTodosAsync(int pagina, int tamanioPagina);

    Task<IEnumerable<Paciente>> BuscarPacientes(string valor, int pagina, int tamanioPagina);

    Task<int> ContarTodosAsync();

    Task<int> ContarPacientesPorBusquedaAsync(string valor);

    Task CrearAsync(Paciente paciente);

    Task ActualizarAsync(Paciente paciente);

    Task<int> EliminarAsync(int id);
}