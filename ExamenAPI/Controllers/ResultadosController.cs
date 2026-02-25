using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Examenes.API.Data;
using Examenes.Modelos.Entidades;

namespace Examenes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultadosController : ControllerBase
    {
        private readonly ExamenApiContext _context;

        public ResultadosController(ExamenApiContext context)
        {
            _context = context;
        }

        // GET: api/Resultados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResultadoExamen>> GetResultado(int id)
        {
            var resultado = await _context.ResultadosExamenes
                .Include(r => r.Parametro) // Trae el catálogo (Glucosa, etc.)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (resultado == null) return NotFound(new { mensaje = "Resultado no encontrado." });

            return resultado;
        }

        // GET: api/Resultados/examen/5
        // Útil para ver el detalle de un examen específico en el MVC
        [HttpGet("examen/{examenId}")]
        public async Task<ActionResult<IEnumerable<ResultadoExamen>>> GetResultadosPorExamen(int examenId)
        {
            return await _context.ResultadosExamenes
                .Include(r => r.Parametro) // Cargamos el nombre y rangos del catálogo
                .Where(r => r.ExamenId == examenId)
                .ToListAsync();
        }

        // PUT: api/Resultados/5
        // Se usa para corregir un valor obtenido si hubo un error de digitación
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResultado(int id, ResultadoExamen resultado)
        {
            if (id != resultado.Id)
            {
                return BadRequest(new { mensaje = "El ID no coincide con el registro." });
            }

            _context.Entry(resultado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResultadoExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/Resultados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResultado(int id)
        {
            var resultado = await _context.ResultadosExamenes.FindAsync(id);
            if (resultado == null) return NotFound();

            _context.ResultadosExamenes.Remove(resultado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResultadoExists(int id)
        {
            return _context.ResultadosExamenes.Any(e => e.Id == id);
        }
    }
}