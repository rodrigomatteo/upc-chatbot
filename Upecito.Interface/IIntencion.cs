using System;
using System.Collections.Generic;
using System.Text;
using Upecito.Model;

namespace Upecito.Interface
{
    public interface IIntencion
    {
        Intencion ObtenerCategoria(string intent);
    }
}
