using Microsoft.AspNetCore.Mvc;
using Examenes.MVC.Servicios;
using Examenes.Modelos.Enums;

namespace Examenes.MVC.Controllers
{
    public class ParametrosController : Controller
    {
        private readonly ApiService _api;
        public ParametrosController(ApiService api) { _api = api; }

        public async Task<IActionResult> Index(TipoExamen tipo = TipoExamen.Sangre)
        {
            return View(await _api.GetParametrosPorTipo(tipo));
        }
    }
}