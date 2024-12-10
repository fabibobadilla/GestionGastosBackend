namespace GestionGastos.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    }
}
