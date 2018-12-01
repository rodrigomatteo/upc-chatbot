using Upecito.Model;

namespace Upecito.Data.Interface
{
    public interface ISolicitudData
    {
        Solicitud Crear(int idCanalAtencion, long idAlumno, int? idCurso, long? idSesion, string consulta, string usuario);
        Solicitud Atender(long idSolicitud, long? idIntencion, string solucion, string estado, string usuario, int? idCurso, int? idActividad, int? idEmpleado, int? cumpleSLA);
        Solicitud Leer(long idSesion);
    }
}
