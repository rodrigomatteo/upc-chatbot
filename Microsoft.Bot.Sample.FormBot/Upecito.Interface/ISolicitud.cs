using System;
using Upecito.Model;

namespace Upecito.Interface
{
    public interface ISolicitud
    {
        Solicitud CrearSolicitud(int idCanalAtencion, long idAlumno, int? idCurso, long? idSesion, string consulta, string usuario);
        Solicitud Actualizar(long idSolicitud, long? idIntencion, string solucion, string estado, string usuario, int? idCurso, int? idActividad, int? idEmpleado, int? cumpleSLA, DateTime? fechaSolucion);
        Solicitud LeerSolicitud(long idSesion);
    }
}
