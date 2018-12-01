using System;

namespace Upecito.Model
{
    public class Solicitud
    {
        public int IdSolicitud { get; set; }
        public int IdCanalAtencion { get; set; }
        public int IdAlumno { get; set; }
        public int? IdCurso { get; set; }
        public int IdSesion { get; set; }
        public string Consulta { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string LogUsuario { get; set; }
        public DateTime LogFecha { get; set; }
        public string Estado { get; set; }
        public int? IdActividad { get; set; }
        public int? CumpleSLA { get; set; }
        public int? IdEmpleado { get; set; }
    }
}
