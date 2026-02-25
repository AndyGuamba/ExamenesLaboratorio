using Examenes.Modelos.Entidades;
using Examenes.MVC.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Examenes.MVC.Controllers
{
    public class PacientesController : Controller
    {
        private readonly ApiService _api;
        public PacientesController(ApiService api) { _api = api; }

        // AÑADE ESTA ACCIÓN PARA SOLUCIONAR EL 404
        public async Task<IActionResult> Index()
        {
            var pacientes = await _api.GetPacientes();
            return View(pacientes);
        }

        // GET: Muestra el formulario vacío
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recibe los datos y guarda
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                var guardado = await _api.PostPaciente(paciente);
                if (guardado)
                {
                    // Si se guarda, lo mandamos a crear un examen
                    return RedirectToAction("Create", "Examenes");
                }
                ModelState.AddModelError("", "Error al guardar el paciente.");
            }
            return View(paciente);
        }
    }
}