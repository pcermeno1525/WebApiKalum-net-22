using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class Jornada
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string JornadaId { get; set;}
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(2, MinimumLength = 1, ErrorMessage = "La cantidad mínima de caracteres es {2} y maxima es {1} para el campo {0}")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "La cantidad mínima de caracteres es {2} y maxima es {1} para el campo {0}")]
        public string Descripcion { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
    }
}