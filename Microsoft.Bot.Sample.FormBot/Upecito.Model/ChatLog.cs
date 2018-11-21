using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Upecito.Model
{
    public class ChatLog
    {
        public int IdChatLog { get; set; }
        public int IdSesion { get; set; }
        public int IdAlumno { get; set; }

        public DateTime Fecha { get; set; }
        public string Texto { get; set; }
        public string Intencion { get; set; }
        public string Fuente { get; set; } //Usuario, Bot
        public string Tipo { get; set; } //DialogFlow, BotFramework
        public string Contexto { get; set; }
        public string Parametros { get; set; }
        public decimal Confianza { get; set; }
    }
}