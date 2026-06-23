namespace Clinica.Application.DTOs.TipoCita
{
    public class TipoCitaDto
    {
        public int Id_TipoCita { get; set; }
        public string Nombre_TipoCita { get; set; } = string.Empty;
        public int Id_Especialidad { get; set; }
    }
}
