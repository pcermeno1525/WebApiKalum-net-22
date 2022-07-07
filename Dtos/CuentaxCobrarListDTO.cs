namespace WebApiKalum.Dtos
{
    public class CuentaxCobrarListDTO
    {
        public string NombreCargo { get; set; }
        public string Anio { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCargo { get; set; }
        public DateTime FechaAplica { get; set; }
        public Decimal Monto { get; set; }
        public Decimal Mora { get; set; }
        public Decimal Descuento { get; set; }

    }
}