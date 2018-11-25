using Google.Cloud.Dialogflow.V2;
using SimpleInjector;
using System;
using Upecito.Interface;
using Upecito.Model;
using FormBot.DependencyResolver;
using System.Text;
using Upecito.Model.Common;

namespace Upecito.Bot.Upecito.Helpers
{
    public class Helpers
    {
        public static Response PersistChatLog(QueryResult response, Sesion sesion, string text, string source, string type)
        {
            Response output = new Response();

            try
            {
                var container = new Container();
                UnityConfig.RegisterTypes(container);
                IChatLog chatlog = container.GetInstance<IChatLog>();

                decimal confidence;
                decimal.TryParse(response?.IntentDetectionConfidence.ToString(), out confidence);

                ChatLog input = new ChatLog
                {
                    IdSesion = (int)sesion.IdSesion,
                    IdAlumno = (int)sesion.IdAlumno,
                    Fecha = DateTime.Now,
                    //Texto = response?.QueryText,
                    Texto = text,
                    Intencion = response?.Intent?.DisplayName,
                    //Fuente = "Usuario",
                    Fuente = source,  //Usuario, Bot
                    Tipo = type, //DialogFlow, BotFramework
                    Contexto = response?.OutputContexts?.ToString(),
                    Parametros = response?.Parameters?.ToString(),
                    Confianza = confidence

                };

                ChatLog chatLogInputData = chatlog.CrearChatLog(input);

                output.Status = true;
                output.Type = Enums.ResponseType.success;
                output.Data = chatLogInputData;

                //ChatLog output = new ChatLog
                //{
                //    IdSesion = (int)sesion.IdSesion,
                //    IdAlumno = (int)sesion.IdAlumno,
                //    Fecha = DateTime.Now,
                //    Texto = response.FulfillmentText,
                //    Intencion = response.Intent.DisplayName,
                //    Fuente = "Bot",
                //    Contexto = response.OutputContexts.ToString(),
                //    Parametros = response.Parameters.ToString(),
                //    Confianza = (decimal)response.IntentDetectionConfidence

                //};

                //ChatLog chatLogOutputData = chatlog.CrearChatLog(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return output;
        }

        public static string EmailTeacher(int idAlumno, string fullNameStudent, string query)
        {
            StringBuilder s = new StringBuilder();

            s.Append("Estimado Docente");

            s.Append($"Buenas tardes, el alumno <strong>{idAlumno} - {fullNameStudent}</strong>; ha remitido la siguiente consulta:");

            s.AppendLine();
            s.Append($"<strong>{query}</strong>");
            s.AppendLine();

            s.Append("Le pedimos brindar una respuesta al alumno respecto a su solicitud.");
            s.AppendLine();


            s.Append("Saludos Cordiales,");


            s.Append("<img src='image.png' alt='UPC' />");
            s.Append($"<strong>UPECITO</strong>");
            s.Append("Asesor del Aula Virtual");

            return s.ToString();
        }

    }
}