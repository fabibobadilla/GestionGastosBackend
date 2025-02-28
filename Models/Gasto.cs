namespace GestionGastos.Models
{
    public class Gasto
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public string? Monto { get; set; }
        public DateTime? FechaCarga { get; set; }
        public int? UsuarioId { get; set; }
        public int CategoriaId { get; set; }
        public string? Tipo { get; set; }

        public Usuario? Usuario { get; set; }
        public Categoria? Categoria { get; set; }
    }
}
