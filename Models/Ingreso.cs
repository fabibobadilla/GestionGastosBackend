namespace GestionGastos.Models
{
    public class Ingreso
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; } // Navegacion
    }
}
