namespace GestionGastos.Models
{
    public class Presupuesto
    {
        public int Id { get; set; }
        public decimal MontoTotal { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }
    }
}
