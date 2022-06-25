using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class Aspirante
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "El campo número de expediente debe ser de 12 caracteres")]
        public string NoExpediente { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Telefono { get; set; }
        [EmailAddress(ErrorMessage = "El correo electrónico no es valido")]
        public string Email { get; set; }
        public string Estatus { get; set; } = "NO ASIGNADO";
        public string CarreraId { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
        public string JornadaId { get; set; }
        public virtual Jornada Jornada { get; set; }
        public string ExamenId { get; set; }
        public virtual ExamenAdmision ExamenAdmision { get; set; }
        public virtual List<InscripcionPago> InscripcionesPagos { get; set; }
        public virtual List<ResultadoExamenAdmision> ResultadosExamenesAdmision { get; set; }

        public Aspirante(string noExpediente, string apellidos, string nombres, string direccion, string telefono, string email, string estatus, string carreraId, string jornadaId, string examenId)
        {
            this.NoExpediente = noExpediente;
            this.Apellidos = apellidos;
            this.Nombres = nombres;
            this.Direccion = direccion;
            this.Telefono = telefono;
            this.Email = email;
            this.Estatus = estatus;
            this.CarreraId = carreraId;
            this.JornadaId = jornadaId;
            this.ExamenId = examenId;

        }

        // public IEnumerable<ValidationResult> Validate(ValidateContext validationContext)
        // {
        //     // bool expedienteValid = false;
        //     if (!string.IsNullOrEmpty(NoExpediente))
        //     {
        //         if (!NoExpediente.Contains("-"))
        //         {
        //             yield return new ValidationResult("El número de expediente no contiene un '-' ", new string[](nameof(NoExpediente)));
        //         }
        //         int guion = NoExpediente.IndexOf("-");
        //         string exp = NoExpediente.Substring(0, guion);
        //         string numero = NoExpediente.Substring(guion + 1, NoExpediente.Length - 4);
        //         if (!exp.ToUpper().Equals("EXP") || !Information.IsNumeric(numero))
        //         {
        //             yield return new ValidationResult("El número de expediente no contiene la nomenclatura adecuada", new string[](nameof(NoExpediente)));
        //         }

        //     }
        // }
    }
}