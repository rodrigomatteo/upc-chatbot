using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormBot
{
    public static class AppConstant
    {
        public static class CanalAtencion
        {
            public const string ConsultasAcademicas = "1";
            public const string ConsultasTecnicas = "2";
        }

        public static class ProgramacionAcademica
        {
            public static class TipoActividad
            {
                public const string Tarea = "1";
                public const string Foro = "2";
                public const string Cuestionario = "3";
                public const string SesionVirtual = "4";
                public const string Evaluacion = "5";                
            }           
        }

        public static class DialogFlow
        {
            public const string FilePrivateKeyIdJson = "upc-chatbot-2b629c2109dc.json";
            public const string ProjectId = "upc-chatbot";
        }

        public static class Intencion
        {
            public const string ASISTENCIA = "Asistencia";
            public const string MATRICULA = "Matricula";
            public const string CREDITOS = "Creditos";
            public const string PROMEDIO = "Promedio";
            public const string RETIRO = "Retiro";
            public const string PROGRAMACION = "ProgramacionAcademica";
            public const string CALENDARIO = "CalendarioAcademico";
            public const string ORGANIZACION = "Organizacion del Aula Virtual";
            public const string DEFAULT = "Default Fallback Intent";            
        }

        public static class EstadoSolicitud
        {
            public const string PENDIENTE = "P";
            public const string ATENDIDO = "A";
            public const string DERIVADA = "D";
            public const string ATENDIDADERIVACION = "R";
            public const string INVALIDO = "I";
            public const string FALTAINFORMACION = "F";
            public const string CANCELADA = "C";
        }
    }
}