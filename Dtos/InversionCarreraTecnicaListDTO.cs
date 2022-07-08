namespace WebApiKalum.Dtos
{
    public class InversionCarreraTecnicaListDTO
    {
        public string InversionId { get; set; }
        public string CarreraId { get; set; }
        public decimal MontoInscripcion { get; set; }
        public int NumeroPagos { get; set; }
        public decimal MontoPago { get; set; }
        public InversionCarreraTecnicaCarreraTecnicaListDTO CarreraTecnica { get; set; }
    }
}