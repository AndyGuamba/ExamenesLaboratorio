using Examenes.Modelos.Entidades;
using Examenes.Modelos.Enums;
using Examenes.MVC.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Examenes.MVC.Controllers
{
    public class ExamenesController : Controller
    {
        private readonly ApiService _api;

        public ExamenesController(ApiService api)
        {
            _api = api;
        }

        // 1. LISTADO PRINCIPAL
        public async Task<IActionResult> Index(TipoExamen tipo = TipoExamen.Sangre)
        {
            ViewBag.TipoActual = tipo;
            var lista = await _api.GetExamenesPorTipo(tipo);
            return View(lista);
        }

        // 2. GET: CREAR EXAMEN (Maneja las pestañas por servidor)
        public async Task<IActionResult> Create(TipoExamen tipo = TipoExamen.Sangre)
        {
            ViewBag.TipoActual = tipo;
            ViewBag.Pacientes = await _api.GetPacientes();
            ViewBag.Parametros = await _api.GetParametrosPorTipo(tipo);

            // Enviamos un examen nuevo con la fecha de hoy y el tipo seleccionado
            return View(new Examen { Tipo = tipo, Fecha = DateTime.Now });
        }

        // 3. POST: GUARDAR EXAMEN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Examen examen, IFormCollection form)
        {
            try
            {
                // Extraemos los resultados dinámicos del formulario
                examen.Resultados = new List<ResultadoExamen>();

                foreach (var key in form.Keys.Where(k => k.StartsWith("resultados[")))
                {
                    var idStr = key.Replace("resultados[", "").Replace("]", "");

                    if (int.TryParse(idStr, out int parametroId) && !string.IsNullOrEmpty(form[key]))
                    {
                        // IMPORTANTE: Asegúrate de usar la cultura invariante si tienes problemas con las comas o puntos decimales
                        examen.Resultados.Add(new ResultadoExamen
                        {
                            ParametroId = parametroId,
                            ValorObtenido = float.Parse(form[key].ToString().Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture)
                        });
                    }
                }

                // Enviamos a la API
                if (await _api.GuardarExamen(examen))
                {
                    return RedirectToAction(nameof(Index), new { tipo = examen.Tipo });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error: " + ex.Message);
            }

            // Si falla, recargamos la vista para no perder la información
            ViewBag.TipoActual = examen.Tipo;
            ViewBag.Pacientes = await _api.GetPacientes();
            ViewBag.Parametros = await _api.GetParametrosPorTipo(examen.Tipo);

            return View(examen);
        }

        // 4. GET: VER REPORTE (Conecta exactamente con tu Details.cshtml)
        public async Task<IActionResult> Details(int id)
        {
            // A. Traemos el examen principal con todo su detalle (Model)
            var examen = await _api.GetExamenPorId(id);
            if (examen == null) return NotFound();

            // B. Traemos el historial para armar el ViewBag.Historial
            var examenesDelMismoTipo = await _api.GetExamenesPorTipo(examen.Tipo);

            // Filtramos para que solo salgan los de la misma cédula y excluimos el que estamos viendo ahorita
            var historial = examenesDelMismoTipo
                .Where(e => e.PacienteCedula == examen.PacienteCedula && e.Id != id)
                .OrderByDescending(e => e.Fecha) // Los más recientes primero
                .ToList();

            ViewBag.Historial = historial;

            // C. Pasamos el modelo a tu vista
            return View(examen);
        }
    }
}