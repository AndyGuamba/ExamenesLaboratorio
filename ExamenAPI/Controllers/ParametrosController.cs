using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Examenes.API.Data;
using Examenes.Modelos.Entidades;
using Examenes.Modelos.Enums;

namespace Examenes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametrosController : ControllerBase
    {
        private readonly ExamenApiContext _context;

        public ParametrosController(ExamenApiContext context)
        {
            _context = context;
        }

        // GET: api/Parametros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParametroExamen>>> GetParametros()
        {
            return await _context.Parametros.ToListAsync();
        }

        // GET: api/Parametros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParametroExamen>> GetParametro(int id)
        {
            var parametro = await _context.Parametros.FindAsync(id);
            if (parametro == null) return NotFound(new { mensaje = "Parámetro no encontrado." });
            return parametro;
        }

        // GET: api/Parametros/categoria/1 (o /categoria/Sangre)
        // CORRECCIÓN: Usamos TipoExamen directamente para que el filtrado sea exacto
        [HttpGet("categoria/{categoria}")]
        public async Task<ActionResult<IEnumerable<ParametroExamen>>> GetParametrosPorCategoria(TipoExamen categoria)
        {
            // El sistema filtra en la base de datos solo los que coinciden con el ID del Enum
            var parametros = await _context.Parametros
                .Where(p => p.Categoria == categoria)
                .ToListAsync();

            return Ok(parametros);
        }

        // POST: api/Parametros
        [HttpPost]
        public async Task<ActionResult<ParametroExamen>> PostParametro(ParametroExamen parametro)
        {
            _context.Parametros.Add(parametro);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetParametro), new { id = parametro.Id }, parametro);
        }

        // PUT: api/Parametros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParametro(int id, ParametroExamen parametro)
        {
            if (id != parametro.Id) return BadRequest();
            _context.Entry(parametro).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Parametros.Any(p => p.Id == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // DELETE: api/Parametros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParametro(int id)
        {
            var parametro = await _context.Parametros.FindAsync(id);
            if (parametro == null) return NotFound();
            _context.Parametros.Remove(parametro);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}