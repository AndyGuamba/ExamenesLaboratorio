using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Examenes.API.Data;
using Examenes.Modelos.Entidades;

namespace Examenes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly ExamenApiContext _context;

        public PacientesController(ExamenApiContext context)
        {
            _context = context;
        }

        // GET: api/Pacientes
        // Obtiene la lista completa de pacientes registrados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paciente>>> GetPacientes()
        {
            return await _context.Pacientes.ToListAsync();
        }

        // GET: api/Pacientes/1752790475
        // Busca un paciente por su cédula
        [HttpGet("{cedula}")]
        public async Task<ActionResult<Paciente>> GetPaciente(string cedula)
        {
            var paciente = await _context.Pacientes.FindAsync(cedula);

            if (paciente == null)
            {
                return NotFound(new { mensaje = "Paciente no encontrado con esa cédula." });
            }

            return paciente;
        }

        // POST: api/Pacientes
        // Registra un nuevo paciente validando que la cédula sea única
        [HttpPost]
        public async Task<ActionResult<Paciente>> PostPaciente(Paciente paciente)
        {
            // Limpieza básica: Eliminar espacios en blanco si los hay
            paciente.Cedula = paciente.Cedula.Trim();

            // Verificamos si ya existe la cédula para evitar error de clave duplicada en SQL
            if (await _context.Pacientes.AnyAsync(p => p.Cedula == paciente.Cedula))
            {
                return BadRequest(new { mensaje = "Ya existe un paciente registrado con esa cédula." });
            }

            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaciente), new { cedula = paciente.Cedula }, paciente);
        }

        // PUT: api/Pacientes/1752790475
        // Actualiza los datos de un paciente existente
        [HttpPut("{cedula}")]
        public async Task<IActionResult> PutPaciente(string cedula, Paciente paciente)
        {
            if (cedula != paciente.Cedula)
            {
                return BadRequest(new { mensaje = "La cédula de la URL no coincide con la del cuerpo de la petición." });
            }

            _context.Entry(paciente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PacienteExists(cedula))
                {
                    return NotFound(new { mensaje = "No se pudo actualizar porque el paciente ya no existe." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Pacientes/1752790475
        // Elimina a un paciente y sus registros asociados (si configuraste borrado en cascada)
        [HttpDelete("{cedula}")]
        public async Task<IActionResult> DeletePaciente(string cedula)
        {
            var paciente = await _context.Pacientes.FindAsync(cedula);
            if (paciente == null)
            {
                return NotFound(new { mensaje = "Paciente no encontrado." });
            }

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PacienteExists(string cedula)
        {
            return _context.Pacientes.Any(e => e.Cedula == cedula);
        }
    }
}