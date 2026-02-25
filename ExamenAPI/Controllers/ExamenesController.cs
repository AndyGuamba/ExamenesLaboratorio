using Examenes.API.Data;
using Examenes.Modelos.Entidades;
using Examenes.Modelos.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Examenes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamenesController : ControllerBase
    {
        private readonly ExamenApiContext _context;

        public ExamenesController(ExamenApiContext context)
        {
            _context = context;
        }

        // GET: api/Examenes/tipo/1 (Sangre)
        // CORRECCIÓN: Usamos TipoExamen directamente para evitar errores de conversión manual
        [HttpGet("tipo/{tipo}")]
        public async Task<ActionResult<IEnumerable<Examen>>> GetExamenesPorTipo(TipoExamen tipo)
        {
            return await _context.Examenes
                .Include(e => e.Paciente)
                .Include(e => e.Resultados)
                    .ThenInclude(r => r.Parametro) // Vital para ver nombres en el Index
                .Where(e => e.Tipo == tipo)
                .ToListAsync();
        }

        // GET: api/Examenes/5
        // CORRECCIÓN: Incluimos todos los niveles para que el MVC no reciba NULLs
        [HttpGet("{id}")]
        public async Task<ActionResult<Examen>> GetExamen(int id)
        {
            var examen = await _context.Examenes
                .Include(e => e.Paciente)
                .Include(e => e.Resultados)
                    .ThenInclude(r => r.Parametro) // Si falta esto, el reporte web falla
                .FirstOrDefaultAsync(e => e.Id == id);

            if (examen == null)
            {
                return NotFound(new { mensaje = "Examen no encontrado." });
            }

            return examen;
        }

        // POST: api/Examenes
        [HttpPost]
        public async Task<ActionResult<Examen>> PostExamen(Examen examen)
        {
            // 1. Validar que el paciente exista
            var pacienteExiste = await _context.Pacientes.AnyAsync(p => p.Cedula == examen.PacienteCedula);
            if (!pacienteExiste)
            {
                return BadRequest(new { mensaje = $"El paciente con cédula {examen.PacienteCedula} no existe." });
            }

            // 2. Limpieza de objeto para evitar errores de rastreo de EF
            // Nos aseguramos de que EF no intente insertar al Paciente de nuevo
            examen.Paciente = null;

            _context.Examenes.Add(examen);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExamen), new { id = examen.Id }, examen);
        }

        // DELETE: api/Examenes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamen(int id)
        {
            var examen = await _context.Examenes.FindAsync(id);
            if (examen == null) return NotFound();

            _context.Examenes.Remove(examen);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}