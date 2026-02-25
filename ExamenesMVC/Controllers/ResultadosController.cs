using Examenes.MVC.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Examenes.MVC.Controllers
{
    public class ResultadosController : Controller
    {
        private readonly ApiService _api;

        public ResultadosController(ApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Details(int id)
        {
            // 1. Traemos el examen principal
            var examen = await _api.GetExamenPorId(id);
            if (examen == null) return NotFound();

            // 2. Traemos el historial para armar el ViewBag.Historial
            var examenesDelMismoTipo = await _api.GetExamenesPorTipo(examen.Tipo);

            // Filtramos para que solo salgan los de la misma cédula y excluimos el actual
            ViewBag.Historial = examenesDelMismoTipo
                .Where(e => e.PacienteCedula == examen.PacienteCedula && e.Id != id)
                .OrderByDescending(e => e.Fecha)
                .ToList();

            // 3. Pasamos el modelo a la vista Resultados/Details.cshtml
            return View(examen);
        }
    }
}