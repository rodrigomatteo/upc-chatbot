using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upecito.Data.Util
{
    public static class UtilConverter
    {
        public static long ConvertirInt64(object value)
        {
            var resultado = 0L;
            try
            {
                resultado = (value == null ? 0 : Convert.ToInt64(value));
            }
            catch (Exception)
            {

            }
            return resultado;
        }
    }
}
