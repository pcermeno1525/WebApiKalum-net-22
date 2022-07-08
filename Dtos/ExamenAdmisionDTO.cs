using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class ExamenAdmisionDTO
    {
        public string ExamenId { get; set; }
        public DateTime FechaExamen { get; set; }
        public virtual List<AspiranteListDTO> Aspirantes { get; set; }
    }
}