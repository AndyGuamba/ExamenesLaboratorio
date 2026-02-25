using Examenes.Modelos.Enums;
using System.ComponentModel.DataAnnotations;

namespace Examenes.Modelos.Entidades
{
    public class ParametroExamen
    {
        [Key]
        public int Id { get; set; }
        public string? Nombre { get; set; } // Ejemplo: Hemoglobina, PH, Glucosa
        public float RangoMinimo { get; set; }
        public float RangoMaximo { get; set; }
        public string? UnidadMedida { get; set; } // Ejemplo: mg/dL, %
        public TipoExamen Categoria { get; set; } // Sangre, Orina o Heces
    }
}