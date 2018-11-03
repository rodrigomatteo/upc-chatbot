using System;
using System.Collections.Generic;
using System.Text;
using Upecito.Model;

namespace Upecito.Interface
{
    public interface ISesion
    {
        Sesion CrearSesion(long idUsuario);
        Sesion CerrarSesion(long idSesion);        
    }
}
