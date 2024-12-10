using GestionGastosShared;
using GestionGastos.Models;
using GestionGastos.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionGastosShared.Services.Interfaces;

namespace GestionGastos.Backend.Services
{
    public class CategoriasService : ICategoriasService
    {
        private readonly GestionGastosContext _context;

        public CategoriasService(GestionGastosContext context)
        {
            _context = context;
        }

        public async Task<List<Categoria>> ObtenerCategorias()
        {
            return await _context.Categorias
                                 .Include(c => c.Usuario)  // Carga la relación con Usuario
                                 .Include(c => c.Gastos)   // Carga la colección de Gastos
                                 .ToListAsync();
        }

        public async Task<Categoria> ObtenerCategoriaPorId(int id)
        {
            return await _context.Categorias
                                 .Include(c => c.Usuario)
                                 .Include(c => c.Gastos)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AgregarCategoria(Categoria nuevaCategoria)
        {
            _context.Categorias.Add(nuevaCategoria);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarCategoria(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }
        }

        Task<List<GestionGastosShared.Models.Categoria>> ICategoriasService.ObtenerCategorias()
        {
            throw new NotImplementedException();
        }

        Task<GestionGastosShared.Models.Categoria> ICategoriasService.ObtenerCategoriaPorId(int id)
        {
            throw new NotImplementedException();
        }

        Task ICategoriasService.AgregarCategoria(GestionGastosShared.Models.Categoria nuevaCategoria)
        {
            throw new NotImplementedException();
        }

        Task ICategoriasService.ActualizarCategoria(GestionGastosShared.Models.Categoria categoria)
        {
            throw new NotImplementedException();
        }

        Task ICategoriasService.EliminarCategoria(int id)
        {
            throw new NotImplementedException();
        }
    }
}
