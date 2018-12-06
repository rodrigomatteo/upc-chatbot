namespace Upecito.Model
{
    public class Alumno
    {
        public int IdAlumno { get; set; }
        public string CodigoAlumno { get; set; }
        public string Unidad { get; set; }
        public int IdPersona { get; set; }
    }

    public class AlumnoUsuarioViewModel
    {
        public int IdAlumno { get; set; }
        public string CodigoAlumno { get; set; }
        public string Unidad { get; set; }
        public int IdPersona { get; set; }

        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
    }
}