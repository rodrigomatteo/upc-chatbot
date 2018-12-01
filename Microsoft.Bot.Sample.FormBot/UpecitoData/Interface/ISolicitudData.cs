using Upecito.Model;

namespace Upecito.Data.Interface
{
    public interface ISolicitudData
    {
        Solicitud Crear(int idCanalAtencion, long idAlumno, int? idCurso, long? idSesion, string consulta, string usuario);
        Solicitud Atender(long idSolicitud, long? idIntencion, string solucion, string estado, string usuario, int? idCurso);
        Solicitud Leer(long idSesion);
    }
}
