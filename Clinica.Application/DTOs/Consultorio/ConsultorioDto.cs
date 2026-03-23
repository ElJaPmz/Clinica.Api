

namespace Clinica.Application.DTOs.Consultorio
{
    public class ConsultorioDto
    {
        public int Id_Consultorio { get; set; }
        public int NumeroConsultorio { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;

    }
}
