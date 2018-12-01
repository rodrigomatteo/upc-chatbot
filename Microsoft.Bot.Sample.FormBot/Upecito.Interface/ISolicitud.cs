using Upecito.Model;

namespace Upecito.Interface
{
    public interface ISolicitud
    {
        Solicitud CrearSolicitud(int idCanalAtencion, long idAlumno, int? idCurso, long? idSesion, string consulta, string usuario);
        Solicitud Actualizar(long idSolicitud, long? idIntencion, string solucion, string estado, string usuario, int? idCurso);
        Solicitud LeerSolicitud(long idSesion);
    }
}
