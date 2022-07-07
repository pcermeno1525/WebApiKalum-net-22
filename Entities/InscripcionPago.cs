using System.ComponentModel.DataAnnotations;
using WebApiKalum.Helpers;

namespace WebApiKalum.Entities
{
    public class InscripcionPago
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string BoletaPago { get; set; }
        [StringLength(12, MinimumLength = 12, ErrorMessage = "El campo número de expediente debe ser de 12 caracteres")]
        [NoExpediente]
        public string NoExpediente { get; set; }
        [StringLength(4, MinimumLength = 4, ErrorMessage = "El campo Ciclo debe ser de 4 caracteres")]
        [CicloAnio]
        public string Anio { get; set; }
        [StringLength(4, MinimumLength = 4, ErrorMessage = "El campo Ciclo debe ser de 4 caracteres")]
        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; }
        [StringLength(4, MinimumLength = 4, ErrorMessage = "El campo Ciclo debe ser de 4 caracteres")]
        [DataType(DataType.Currency)]
        public decimal Monto { get; set; }
        public virtual Aspirante Aspirante { get; set; }
    }
}