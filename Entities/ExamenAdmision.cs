using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class ExamenAdmision
    {
        public string ExamenId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaExamen { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
    }
}