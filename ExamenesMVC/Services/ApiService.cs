using Examenes.Modelos.Entidades; // Asegúrate de que Examen esté en este namespace
using Examenes.Modelos.Enums;
using Newtonsoft.Json;
using System.Text;

namespace Examenes.MVC.Servicios
{
    public class ApiService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl = "https://localhost:7047/api/";

        public ApiService(HttpClient http) { _http = http; }

        // --- PACIENTES ---
        public async Task<List<Paciente>> GetPacientes()
        {
            var resp = await _http.GetAsync($"{_baseUrl}Pacientes");
            return resp.IsSuccessStatusCode ? JsonConvert.DeserializeObject<List<Paciente>>(await resp.Content.ReadAsStringAsync()) : new();
        }

        public async Task<bool> PostPaciente(Paciente p)
        {
            var content = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");
            return (await _http.PostAsync($"{_baseUrl}Pacientes", content)).IsSuccessStatusCode;
        }

        // --- PARÁMETROS (Catalogo) ---
        public async Task<List<ParametroExamen>> GetParametrosPorTipo(TipoExamen tipo)
        {
            // Enviamos el nombre o ID del enum a la API
            var resp = await _http.GetAsync($"{_baseUrl}Parametros/categoria/{tipo}");
            if (resp.IsSuccessStatusCode)
            {
                var content = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ParametroExamen>>(content);
            }
            return new List<ParametroExamen>();
        }

        // --- EXÁMENES ---
        public async Task<bool> GuardarExamen(Examen e)
        {
            var content = new StringContent(JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");
            return (await _http.PostAsync($"{_baseUrl}Examenes", content)).IsSuccessStatusCode;
        }

        public async Task<List<Examen>> GetExamenesPorTipo(TipoExamen tipo)
        {
            var resp = await _http.GetAsync($"{_baseUrl}Examenes/tipo/{tipo}");
            return resp.IsSuccessStatusCode ? JsonConvert.DeserializeObject<List<Examen>>(await resp.Content.ReadAsStringAsync()) : new();
        }

        // MÉTODO NUEVO: Corrige el error CS1061 de tu imagen
        public async Task<Examen?> GetExamenPorId(int id)
        {
            var resp = await _http.GetAsync($"{_baseUrl}Examenes/{id}");
            if (resp.IsSuccessStatusCode)
            {
                var content = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Examen>(content);
            }
            return null;
        }
    }
}