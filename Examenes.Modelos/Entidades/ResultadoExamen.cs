using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examenes.Modelos.Entidades
{
    public class ResultadoExamen
    {
        public int Id { get; set; }
        public int ExamenId { get; set; }
        public virtual Examen? Examen { get; set; }

        // Relación con el nuevo catálogo
        public int ParametroId { get; set; }
        public virtual ParametroExamen? Parametro { get; set; }

        public float ValorObtenido { get; set; }
    }
}
