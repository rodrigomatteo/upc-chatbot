using System;

namespace Upecito.Model
{
    public class Ciclo
    {
        public int IdCiclo { get; set; }
        public string UnidadNegocio { get; set; }
        public int Anio { get; set; }
        public int NumeroCiclo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool IsRegular { get; set; }
        public bool Activo { get; set; }
        public bool Vigente { get; set; }

    }
}