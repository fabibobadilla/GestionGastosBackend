namespace GestionGastos.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime FechaRegistro { get; set; }

        public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();// Relación uno a muchos con Categoría
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>(); // Relación uno a muchos con Gasto
        public ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>(); // Relación uno a muchos con Ingreso
        public ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>(); // Relación uno a muchos con Presupuesto
    }
}
