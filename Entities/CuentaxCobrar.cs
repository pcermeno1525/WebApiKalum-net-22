using System.ComponentModel.DataAnnotations;
using WebApiKalum.Helpers;

namespace WebApiKalum.Entities
{
    public class CuentaxCobrar
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NombreCargo { get; set;}
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "El campo Anio debe ser de 4 caracteres")]
        [CicloAnio]
        public string Anio { get; set;}
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Carne { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string CargoId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaCargo { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaAplica { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Currency)]
        public decimal Monto { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Currency)]
        public decimal Mora { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Currency)]
        public decimal Descuento { get; set; }
        public virtual Cargo Cargo { get; set; }       
        public virtual Alumno Alumno { get; set; }

    }
}