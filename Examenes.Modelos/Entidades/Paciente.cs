using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examenes.Modelos.Entidades
{
        public class Paciente
        {
           [Key] public string? Cedula { get; set; }
            public string? Nombre { get; set; }
            public string? Apellido { get; set; }
            public DateTime FechaNacimiento { get; set; }
            public string? Genero { get; set; } // Masculino/Femenino
        }
}
