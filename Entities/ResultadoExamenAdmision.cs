using System.ComponentModel.DataAnnotations;
using WebApiKalum.Helpers;

namespace WebApiKalum.Entities
{
    public class ResultadoExamenAdmision
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [NoExpediente]
        public string NoExpediente { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [CicloAnio]
        public string Anio { get; set; }
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^\d+$")]
        public int Nota { get; set; }
        public virtual Aspirante Aspirante { get; set; }
    }
}