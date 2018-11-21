using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FormBot.Util
{
    public static class ConvertidorUtil
    {
        public static string FormatearFechaHora(DateTime? date)
        {
            var resultado = string.Empty;
            try
            {
                if (resultado != null)
                    resultado = string.Format("{0:dd/MM/yyyy HH:mm}", date);
                if (date == DateTime.MinValue)
                    resultado = string.Empty;
            }
            catch (Exception)
            {

            }
            return resultado;
        }

        public static string FormatearFechaEsp(DateTime? date)
        {
            var resultado = string.Empty;
            try
            {
                if (resultado != null)
                    resultado = string.Format("{0:dd/MM/yyyy}", date);
                if (date == DateTime.MinValue)
                    resultado = string.Empty;
            }
            catch (Exception)
            {

            }
            return resultado;
        }

        public static DateTime? GetDateTime(object value)
        {
            DateTime? resultado = null;
            try
            {
                resultado = (DateTime)value;
            }
            catch (Exception)
            {

            }
            return resultado;
        }

        public static DateTime? ConvertirDateTime(string strDateTime)
        {
            DateTime? resultado = null;

            try
            {
                resultado = DateTime.ParseExact(strDateTime, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw;
            }
            return resultado;

        }

        public static DateTime? ConvertirDateTimeShort(string strDateTime)
        {
            DateTime? resultado = null;

            try
            {
                resultado = DateTime.ParseExact(strDateTime, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw;
            }
            return resultado;

        }

        public static string FormatearFecha(DateTime? date)
        {
            var resultado = string.Empty;
            try
            {
                if (resultado != null)
                    resultado = string.Format("{0:dd/MM/yyyy HH:mm}", date);
                if (date == DateTime.MinValue)
                    resultado = string.Empty;
            }
            catch (Exception)
            {

            }
            return resultado;
        }
        
        public static DateTime? ConvertirDateTime(object value, char caracter)
        {
            DateTime? resultado = null;
            try
            {
                var fecha = value.ToString();
                var arreglo = fecha.Split(caracter);

                int dia = int.Parse(arreglo[0]);
                int mes = int.Parse(arreglo[1]);
                int anio = int.Parse(arreglo[2]);

                resultado = new DateTime(anio, mes, dia);
            }
            catch (Exception)
            {

            }
            return resultado;
        }

        public static string ConvertirString(object value)
        {
            var resultado = string.Empty;
            try
            {
                resultado = value.ToString();
            }
            catch (Exception)
            {

            }
            return resultado;
        }

        public static decimal ConvertirDecimal(object value)
        {
            var resultado = 0M;
            try
            {
                resultado = value == null ? 0M : Convert.ToDecimal(value);
            }
            catch (Exception)
            {

            }
            return resultado;
        }

        public static int ConvertirInt(object value)
        {
            var resultado = 0;
            try
            {

                if ("true".Equals(value)) value = 1;
                if ("false".Equals(value)) value = 1;

                resultado = int.Parse(value.ToString());
            }
            catch (Exception)
            {

            }
            return resultado;
        }

        public static string FormatoDecimal(decimal numero)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:N}", numero);
        }

        public static string ObtenerPeriodoActual()
        {
            var resultado = string.Empty;

            var fecha = ConvertidorUtil.GmtToPacific(DateTime.Now);
            
            var mes = string.Empty;
            switch (fecha.Month)
            {
                case 1:
                    mes = "Enero";
                    break;
                case 2:
                    mes = "Febrero";
                    break;
                case 3:
                    mes = "Marzo";
                    break;
                case 4:
                    mes = "Abril";
                    break;
                case 5:
                    mes = "Mayo";
                    break;
                case 6:
                    mes = "Junio";
                    break;
                case 7:
                    mes = "Julio";
                    break;
                case 8:
                    mes = "Agosto";
                    break;
                case 9:
                    mes = "Septiembre";
                    break;
                case 10:
                    mes = "Octubre";
                    break;
                case 11:
                    mes = "Noviembre";
                    break;
                case 12:
                    mes = "Diciembre";
                    break;

            };

            resultado = string.Format("{0}-{1}", mes, fecha.Year);

            return resultado;
        }

        public static DateTime GmtToPacific(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime,TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));
        }

        public static DataTable ToDataTable<T>(this IList<T> data, string tableName, List<string> columns = null)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));

            var table = new DataTable(tableName);
            var tableMerge = new DataTable("Merge");

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (columns != null)
                {
                    var column = columns.Where(q => q.Equals(prop.Name)).FirstOrDefault();
                    if (column != null)
                    {
                        tableMerge.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    }
                }

                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }

            if (columns != null)
            {
                tableMerge.Merge(table, true, MissingSchemaAction.Ignore);
                return tableMerge;
            }
            else
            {
                return table;
            }
        }
        
        public static string ConvertMes(string mes)
        {
            var mes_ = string.Empty;

            if (mes.Equals("Enero")) mes_ = "1";
            if (mes.Equals("Febrero")) mes_ = "2";
            if (mes.Equals("Marzo")) mes_ = "3";
            if (mes.Equals("Abril")) mes_ = "4";
            if (mes.Equals("Mayo")) mes_ = "5";
            if (mes.Equals("Junio")) mes_ = "6";
            if (mes.Equals("Julio")) mes_ = "7";
            if (mes.Equals("Agosto")) mes_ = "8";
            if (mes.Equals("Septiembre")) mes_ = "9";
            if (mes.Equals("Octubre")) mes_ = "10";
            if (mes.Equals("Noviembre")) mes_ = "11";
            if (mes.Equals("Diciembre")) mes_ = "12";

            return mes_;
        }

        public static string GetStringFecha()
        {
            var result = string.Empty;

            try
            {
                result = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);   
            }

            return result;
        }

        public static long GetLong(string value)
        {
            var resultado = 0L;
            try
            {               
                resultado = Convert.ToInt64(value);
            }
            catch (Exception)
            {

            }
            return resultado;
        }
    }
}
