﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionGastos.DataContext;
using GestionGastos.Models;


namespace GestionGastos.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly GestionGastosContext _context;

        public CategoriasController(GestionGastosContext context)
        {
            _context = context;
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }

        // GET: api/Categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        // PUT: api/Categorias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categorias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<IActionResult> AgregarCategoria([FromBody] Categoria categoria)
        //{
        //    if (categoria == null || string.IsNullOrWhiteSpace(categoria.Nombre) || categoria.UsuarioId <= 0)
        //    {
        //        return BadRequest("El modelo de categoría no es válido.");
        //    }

        //    // Lógica para guardar la categoría
        //    await _context.Categorias.AddAsync(categoria);
        //    await _context.SaveChangesAsync();

        //    return Ok(categoria);
        //}


        [HttpPost]
        public async Task<ActionResult<string>> PostCategoria(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo inválido.");
            }

            try
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();
                return Ok("Creado exitosamente!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            //return CreatedAtAction("GetCategoria", new { id = categoria.Id }, categoria);
        }

        // DELETE: api/Categorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}