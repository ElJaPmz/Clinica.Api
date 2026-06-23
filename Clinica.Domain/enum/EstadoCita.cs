namespace Clinica.Domain.Enums
{
    public enum EstadoCita
    {
        // La cita ha sido agendada pero no confirmada aún
        Pendiente = 0,

        // El personal administrativo o el sistema confirmó la cita
        Confirmada = 1,

        // El paciente ya está en el consultorio siendo atendido
        Atendida = 2,

        // La cita se completó exitosamente
        Finalizada = 3,

        // El paciente o la clínica cancelaron la cita
        Cancelada = 4,

        // El paciente no se presentó a la hora acordada
        Ausente = 5
    }
}