using System.ComponentModel.DataAnnotations;
using WebApiKalum.Helpers;

namespace WebApiKalum.Entities
{
    public class Inscripcion
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string InscripcionId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        // [Carne]
        public string Carne { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string CarreraId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string JornadaId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "El campo Ciclo debe ser de 4 caracteres")]
        [CicloAnio]
        public string Ciclo { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public virtual Jornada Jornada { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
        public virtual Alumno Alumno { get; set; }
    }
}