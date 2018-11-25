using System;

namespace Upecito.Model
{
    public class Actividad
    {
        public int IdActividad { get; set; }
        public int IdTipoActividad { get; set; }
        public int IdSeccion { get; set; }
        public int NumeroActividad { get; set; }
        public DateTime FechaActividad { get; set; }
    }
}