using Examenes.Modelos.Entidades;
using Examenes.Modelos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examenes.Modelos.Entidades
{
    public class Examen
    {
            public int Id { get; set; }
            public DateTime Fecha { get; set; }

            public string? PacienteCedula { get; set; }
            public virtual Paciente? Paciente { get; set; }

            public TipoExamen Tipo { get; set; }
            public virtual List<ResultadoExamen> Resultados { get; set; } = new List<ResultadoExamen>();   
    }
}