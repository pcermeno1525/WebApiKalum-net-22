using System.ComponentModel.DataAnnotations;
namespace WebApiKalum.Helpers
{
    public class NoExpedienteAttribute : ValidationAttribute
    {
        // protected override ValidationResult IsValid(object value, ValidationContext context)
        // {
        //     if (!string.IsNullOrEmpty(NoExpediente))
        //     {
        //         return ValidationResult.Success;
        //     }
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

        //     return ValidationResult.Success;

        // }
    }
}