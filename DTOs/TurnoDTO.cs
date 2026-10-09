
namespace SistemaTurnosCentroUnas.DTOs
{
    public class TurnoDTO
    {
        public int Id { get; set; }

        public DateTime FechaHora { get; set; }

        public string Servicio { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int ClienteId { get; set; }
    }
}