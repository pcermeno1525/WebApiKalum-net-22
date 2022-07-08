using System.ComponentModel.DataAnnotations;
using WebApiKalum.Helpers;

namespace WebApiKalum.Entities
{
    public class Alumno
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El campo Carne debe ser de 8 caracteres")]
        // [Carne]
        public string Carne { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Direccion { get; set;}
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Telefono { get; set; }
        [EmailAddress(ErrorMessage = "El correo electrónico no es valido")]
        public string Email { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
        public virtual List<CuentaxCobrar> CuentasxCobrar { get; set; }
        
    }
}