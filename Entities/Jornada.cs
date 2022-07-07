using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class Jornada
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string JornadaId { get; set;}
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Descripcion { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
    }
}