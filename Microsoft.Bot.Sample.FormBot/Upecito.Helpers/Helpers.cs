using Google.Cloud.Dialogflow.V2;
using SimpleInjector;
using System;
using Upecito.Interface;
using Upecito.Model;
using FormBot.DependencyResolver;
using System.Text;
using Upecito.Model.Common;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using FormBot.Dialogflow;
using FormBot;
using FormBot.Dialogs;
using System.Collections.Generic;
using Newtonsoft.Json;
using FormBot.Services;
using Upecito.Model.ViewModel;
using System.Linq;
using Upecito.Data.Implementation;
using System.Web;
using System.Threading;
using FormBot.Util;

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

        public static string EmailTeacher(string codigoAlumno, string fullNameStudent, string query)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<html><body>");
            sb.Append("Estimado Docente,");

            sb.Append("<br />");
            sb.Append("<br />");

            sb.Append($"Buenas tardes, el alumno <strong>{codigoAlumno} - {fullNameStudent}</strong>, ha remitido la siguiente consulta:");

            sb.Append("<br />");
            sb.Append("<br />");

            sb.Append($"<strong>{query}</strong>");

            sb.Append("<br />");
            sb.Append("<br />");

            sb.Append("Le pedimos brindar una respuesta al alumno respecto a su solicitud.");

            sb.Append("<br />");
            sb.Append("<br />");

            sb.Append("Saludos Cordiales,");

            sb.Append("<br />");
            sb.Append("<br />");

            sb.Append($"<img src='https://upecito.azurewebsites.net/content/images/upc_logo.png' alt='UPC' />");

            sb.Append("<strong>UPECITO</strong>");
            sb.Append("<br />");
            sb.Append("Asesor del Aula Virtual");

            sb.Append("</body></html>");

            return sb.ToString();
        }

        public static async Task Process(IDialogContext context)
        {
            var activity = context.Activity as Activity;

            Sesion sesion = context.UserData.GetValue<Sesion>("sesion");
            context.UserData.SetValue("Usuario", sesion.Nombre);

            var container = new Container();
            UnityConfig.RegisterTypes(container);

            /* 4.1.4   El Sistema crea una nueva Solicitud Académica con los datos indicados líneas abajo
                en la entidad[GSAV_SolicitudAcadémica], generando un código único */

            Solicitud solicitud = new Solicitud();

            if (context.UserData.ContainsKey("solicitud"))
            {
                solicitud = LeerSolicitud(sesion, context, activity, container);

                if (solicitud.Estado != "P")
                {
                    solicitud = CrearNuevaSolicitud(sesion, context, activity, container);
                }
            }
            else
            {
                solicitud = CrearNuevaSolicitud(sesion, context, activity, container);
            }

            try
            {
                DialogEngine handler = container.GetInstance<DialogEngine>();

                Result receivedResult = await handler.GetSpeechAsync(activity, sesion, context);

                IIntencion intencionManager = container.GetInstance<IIntencion>();
                /*
                 * 4.1.5   El Sistema valida si existe una “Intención de Consulta” para la pregunta 
                 * ingresada por el alumno a través del Agente de Procesamiento de Lenguaje Natural. 
                 * GSAV _RN013 – Tipos de Consultas Académicas
                */
                if (receivedResult.ExistIntent)
                {
                    /*
                     * 4.1.6   El Sistema valida si la “Intención de Consulta” obtenida tiene una probabilidad 
                     * mayor o igual al 80 %.GSAV _RN018– Porcentaje para respuestas certera
                    */

                    var intentScore = receivedResult.Intents.FirstOrDefault().Score;

                    if (receivedResult.ExistValidIntent)
                    {
                        string intent = receivedResult.GetValidIntent();
                        Intencion intencion = intencionManager.ObtenerCategoria(intent);

                        context.UserData.SetValue("result", receivedResult);

                        if (string.IsNullOrEmpty(intencion.NombreBase))
                        {
                            intencion.NombreBase = intent;
                        }

                        context.UserData.SetValue("intencion", intencion);

                        if (!string.IsNullOrEmpty(intent))
                        {
                            string dfParams = receivedResult.Intents[0].Parameters;
                            string dfContext = receivedResult.OutputContexts;
                            Dictionary<string, string> listParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(dfParams);
                            string course = string.Empty;
                            string assignment = string.Empty;
                            int number = 0;

                            if (listParams.Count > 0)
                            {
                                if (listParams.ContainsKey("Curso"))
                                {
                                    course = listParams["Curso"];
                                    context.UserData.SetValue("Curso", course);
                                }

                                if (listParams.ContainsKey("Tarea"))
                                {
                                    assignment = listParams["Tarea"];
                                    context.UserData.SetValue("Tarea", assignment);
                                }

                                if (listParams.ContainsKey("number"))
                                {
                                    int.TryParse(listParams["number"], out number);
                                    context.UserData.SetValue("number", number);
                                }
                            }

                            var message = context.MakeMessage();

                            switch (intencion.NombreBase)
                            {
                                case "ActividadAcademica":
                                    message.Text = $"Esta es una consulta de: {intent}";

                                    context.PrivateConversationData.SetValue("custom", message.Text);

                                    await context.PostAsync(message);

                                    await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.ATENDIDO);
                                    context.Wait(MenuDialog.MessageReceivedAsync);
                                    break;

                                case AppConstant.Intencion.PROGRAMACION:

                                    if (receivedResult.Intents[0].AllRequiredParamsPresent)
                                    {
                                        string fechaActividad = string.Empty;
                                        int numberSelected = context.UserData.GetValue<int>("number");

                                        var actividadManager = container.GetInstance<IActividad>();

                                        List<ActivitiesByCourseViewModel> activities = actividadManager.GetActivitiesByCourse(solicitud.IdAlumno);

                                        var filtered = activities.Where(o => o.Curso == course && o.Actividad == assignment);

                                        ActivitiesByCourseViewModel a = new ActivitiesByCourseViewModel();

                                        if (filtered.Count() == 0)
                                        {
                                            //context.Call(new NoRespuestaDialog(), MenuDialog.ResumeAfterDerivedAcademicIntent);

                                            await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.DERIVADA);
                                            context.Wait(MenuDialog.MessageReceivedAsync);

                                            return;
                                        }

                                        if (numberSelected > 0)
                                        {
                                            a = filtered.Where(o => o.NumeroActividad == numberSelected).FirstOrDefault();

                                            fechaActividad = a?.FechaActividad.ToString("dd MMMM yyyy");
                                        }
                                        else
                                        {
                                            a = filtered.Where(o => o.FechaActividad >= DateTime.Now).FirstOrDefault();

                                            fechaActividad = a?.FechaActividad.ToString("dd MMMM yyyy");
                                        }

                                        if (a == null)
                                        {
                                            a = new ActivitiesByCourseViewModel();
                                        }

                                        context.UserData.SetValue("Actividad", a);


                                        if (!string.IsNullOrEmpty(fechaActividad))
                                        {

                                            if (string.IsNullOrEmpty(receivedResult.Speech))
                                            {
                                                string tareaConNumero = assignment;

                                                if (number > 0)
                                                {
                                                    tareaConNumero = $"{assignment} {number}";
                                                }

                                                receivedResult.Speech = $"La fecha de actividad de la {tareaConNumero} del Curso {course} es: {fechaActividad}";
                                            }
                                            else
                                            {
                                                receivedResult.Speech = receivedResult.Speech + " " + fechaActividad;
                                            }


                                            context.UserData.SetValue("result", receivedResult);

                                            await context.PostAsync(receivedResult.Speech);

                                            //await context.Forward(new MenuDialog(), MenuDialog.ResumeAfterSuccessAcademicIntent, context, CancellationToken.None);
                                            //context.Wait(MenuDialog.ResumeAfterSuccessAcademicIntent);
                                            //context.Call(new MenuDialog(), MenuDialog.ResumeAfterSuccessAcademicIntent);

                                            await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.ATENDIDO);
                                            context.Wait(MenuDialog.MessageReceivedAsync);
                                        }
                                        else
                                        {
                                            //context.Wait(MenuDialog.ResumeAfterFailedAcademicIntent);
                                            //await context.PostAsync(receivedResult.Speech);
                                            await context.PostAsync("La información ingresada es invalida, ingrese nuevamente su consulta");

                                            //context.Call(new NoRespuestaDialog(), MenuDialog.ResumeAfterDerivedAcademicIntent);

                                            //await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.FALTAINFORMACION);

                                            //context.Wait(MenuDialog.MessageReceivedAsync);

                                            //context.Call(new ProgramacionActividadesDialog(), MenuDialog.ResumeAfterSuccessAcademicIntent);

                                            await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.INVALIDO);
                                            context.Wait(MenuDialog.MessageReceivedAsync);
                                        }

                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(course))
                                        {

                                            var cursoManager = container.GetInstance<ICurso>();

                                            List<CourseByModuleViewModel> studentActiveCourses = cursoManager.GetCourseByModuleActive(solicitud.IdAlumno);

                                            var questions = studentActiveCourses.Select(x => x.Curso).ToList(); ;

                                            string QuestionPrompt = "Por favor seleccione el curso?";
                                            string invalidCourseInput = "La información ingresada es invalida, ingrese nuevamente su consulta";

                                            PromptOptions<string> options = new PromptOptions<string>(QuestionPrompt, "", invalidCourseInput, questions, 0); // Overrided the PromptOptions Constructor.
                                            PromptDialog.Choice(context, MenuDialog.OnCourseSelected, options);

                                        }
                                        else
                                        {
                                            await context.PostAsync(receivedResult.Speech);
                                        }

                                        context.Wait(MenuDialog.MessageReceivedAsync);

                                    }

                                    break;

                                case AppConstant.Intencion.CREDITOSCUSTOM:
                                    if (receivedResult.Intents[0].AllRequiredParamsPresent)
                                    {
                                        var actividadManager = container.GetInstance<IActividad>();

                                        List<ActivitiesByCourseViewModel> activities = actividadManager.GetActivitiesByCourse(solicitud.IdAlumno);

                                        var filtered = activities.Where(o => o.Curso == course);


                                        await context.PostAsync(receivedResult.Speech + " " + 0);
                                        //context.Wait(MenuDialog.ResumeAfterSuccessAcademicIntent);
                                        //context.Call(new MenuDialog(), MenuDialog.ResumeAfterSuccessAcademicIntent);
                                        context.Wait(MenuDialog.MessageReceivedAsync);

                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(course))
                                        {

                                            var cursoManager = container.GetInstance<ICurso>();

                                            List<CourseByModuleViewModel> studentActiveCourses = cursoManager.GetCourseByModuleActive(solicitud.IdAlumno);

                                            var questions = studentActiveCourses.Select(x => x.Curso).ToList(); ;

                                            string QuestionPrompt = "Por favor seleccione el curso?";
                                            string invalidCourseInput = "La información ingresada es invalida, ingrese nuevamente su consulta";

                                            PromptOptions<string> options = new PromptOptions<string>(QuestionPrompt, "", invalidCourseInput, questions, 0); // Overrided the PromptOptions Constructor.
                                            PromptDialog.Choice(context, MenuDialog.OnCourseSelected, options);

                                        }
                                        else
                                        {
                                            await context.PostAsync(receivedResult.Speech);
                                        }

                                        context.Wait(MenuDialog.MessageReceivedAsync);
                                    }

                                    break;

                                case AppConstant.Intencion.CREDITOS:
                                case AppConstant.Intencion.ORGANIZACION:
                                case AppConstant.Intencion.CALENDARIO:
                                    //case "OrganizacionAulaVirtual1":
                                    //context.Call(new CalendarioDialog(), ResumeAfterSuccessAcademicIntent);
                                    message = context.MakeMessage();
                                    message.Text = $"Esta es una consulta de: {intent}";

                                    //receivedResult.Speech = message.Text;
                                    context.PrivateConversationData.SetValue("custom", message.Text);

                                    await context.PostAsync(message);
                                    //context.Wait(MenuDialog.MessageReceivedAsync);
                                    //context.Call(new MenuDialog(), ResumeAfterSuccessAcademicIntent);
                                    //context.Wait(MenuDialog.ResumeAfterSuccessAcademicIntent);
                                    //context.Call(new MenuDialog(), MenuDialog.ResumeAfterSuccessAcademicIntent);

                                    await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.ATENDIDO);
                                    context.Wait(MenuDialog.MessageReceivedAsync);
                                    break;

                                //case AppConstant.Intencion.ORGANIZACION:
                                //    context.Call(new OrganizacionDialog(), ResumeAfterSuccessAcademicIntent);
                                //    break;

                                case AppConstant.Intencion.MATRICULA:
                                case AppConstant.Intencion.ASISTENCIA:
                                case AppConstant.Intencion.RETIRO:
                                case AppConstant.Intencion.PROMEDIO:
                                    //context.Call(new PreguntasFrecuentesDialog(), MenuDialog.ResumeAfterSuccessAcademicIntent);

                                    //if (receivedResult.Speech.Equals(string.Empty))
                                    //{
                                    //    receivedResult.Speech = "Por favor, canalice su consulta vía Contacto UPC: http://www.upc.edu.pe/servicios/contacto-upc";
                                    //}

                                    await context.PostAsync(receivedResult.Speech);

                                    await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.ATENDIDO);
                                    context.Wait(MenuDialog.MessageReceivedAsync);

                                    break;

                                //case AppConstant.Intencion.CREDITOS:
                                //    context.Call(new CreditosDialog(), ResumeAfterSuccessAcademicIntent);
                                //    break;
                                case AppConstant.Intencion.DEFAULT:

                                    if (receivedResult.Speech.Equals(string.Empty))
                                    {
                                        receivedResult.Speech = "Por favor, canalice su consulta vía Contacto UPC: http://www.upc.edu.pe/servicios/contacto-upc";
                                    }

                                    await context.PostAsync(receivedResult.Speech);

                                    await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.INVALIDO);
                                    context.Wait(MenuDialog.MessageReceivedAsync);

                                    //context.Call(new NoRespuestaDialog(), MenuDialog.ResumeAfterFailedAcademicIntent);
                                    break;


                                default:
                                    //context.Call(new NoRespuestaDialog(), MenuDialog.ResumeAfterDerivedAcademicIntent);

                                    await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.DERIVADA);
                                    context.Wait(MenuDialog.MessageReceivedAsync);
                                    break;
                            }
                        }
                        else
                        {
                            var userName = context.UserData.GetValue<Sesion>("sesion").Nombre;
                            var message = context.MakeMessage();
                            message.Text = $"Uhmmm... {userName} estoy entrenándome para ayudarte más adelante con este tipo de dudas.{Environment.NewLine} Pero recuerda que vía Contacto UPC:  http://www.upc.edu.pe/servicios/contacto-upc puedes resolver tus dudas o consultas.";

                            context.PrivateConversationData.SetValue("custom", message.Text);

                            await context.PostAsync(message);

                            await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.INVALIDO);

                            //context.Wait(MenuDialog.ResumeGetAcademicIntent);
                            context.Wait(MenuDialog.MessageReceivedAsync);
                        }
                    }
                    else
                    {
                        //ExistValidIntent = false

                        //context.Call(new SinScoreDialog(), MenuDialog.ResumeAfterDerivedAcademicIntent);

                        var userName = context.UserData.GetValue<Sesion>("sesion").NombreApePaterno;

                        var message = context.MakeMessage();
                        message.Text = $"Uhmmm... {userName} estoy entrenándome para ayudarte más adelante con este tipo de dudas.{Environment.NewLine} Pero recuerda que vía Contacto UPC:  http://www.upc.edu.pe/servicios/contacto-upc puedes resolver tus dudas o consultas.";

                        context.PrivateConversationData.SetValue("custom", message.Text);

                        await context.PostAsync(message);

                        //context.Call(new MenuDialog(), MenuDialog.ResumeAfterUnknownAcademicIntent);
                        await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.INVALIDO);
                        context.Wait(MenuDialog.MessageReceivedAsync);
                    }
                }
                else
                { //ExistIntent = false
                  //await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.INVALIDO);
                  //context.Call(new SinScoreDialog(), MenuDialog.ResumeAfterUnknownAcademicIntent);
                    var userName = context.UserData.GetValue<Sesion>("sesion").NombreApePaterno;

                    string sinScoreDialogSpeech =
                         $"Uhmmm... {userName} estoy entrenándome para ayudarte más adelante con este tipo de dudas.{Environment.NewLine}. Pero recuerda que vía Contacto UPC: http://www.upc.edu.pe/servicios/contacto-upc puedes resolver tus dudas o consultas";

                    await context.PostAsync(sinScoreDialogSpeech);
                    //context.Call(new MenuDialog(), MenuDialog.ResumeAfterUnknownAcademicIntent);

                    await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.INVALIDO);
                    context.Wait(MenuDialog.MessageReceivedAsync);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                BaseData.LogError(ex);
            }

        }

        private static Solicitud CrearNuevaSolicitud(Sesion sesion, IDialogContext context, Activity activity, Container container)
        {
            var userId = sesion.IdAlumno;
            var codigoAlumno = sesion.CodigoAlumno;
            var idSesion = sesion.IdSesion;

            var tipoConsulta = context.UserData.GetValue<string>("tipo-consulta");

            var solicitudManager = container.GetInstance<ISolicitud>();
            Solicitud solicitud = solicitudManager.CrearSolicitud(Convert.ToInt32(tipoConsulta), userId, null, idSesion, activity.Text, codigoAlumno);

            //context.UserData.SetValue("OutputContexts", string.Empty);
            //context.UserData.SetValue("result", string.Empty);
            //context.UserData.SetValue("intencion", string.Empty);

            context.UserData.SetValue("solicitud", solicitud);

            context.UserData.SetValue("Curso", string.Empty);
            context.UserData.SetValue("Tarea", string.Empty);
            context.UserData.SetValue("number", string.Empty);

            context.UserData.SetValue("Actividad", new ActivitiesByCourseViewModel());

            return solicitud;
        }

        private static Solicitud LeerSolicitud(Sesion sesion, IDialogContext context, Activity activity, Container container)
        {
            var idSesion = sesion.IdSesion;


            var solicitudManager = container.GetInstance<ISolicitud>();
            Solicitud solicitud = solicitudManager.LeerSolicitud(idSesion);

            context.UserData.SetValue("solicitud", solicitud);

            return solicitud;
        }

        public static async Task ActualizarSolicitud(IDialogContext context, string estado)
        {
            try
            {



                var container = new Container();
                UnityConfig.RegisterTypes(container);

                var solicitudManager = container.GetInstance<ISolicitud>();
                Solicitud solicitud = context.UserData.GetValueOrDefault<Solicitud>("solicitud");
                Sesion sesion = context.UserData.GetValue<Sesion>("sesion");
                string curso = context.UserData.GetValue<string>("Curso");
                string userName = sesion.CodigoAlumno;

                Result receivedResult;
                context.UserData.TryGetValue("result", out receivedResult);

                Intencion intent;
                context.UserData.TryGetValue("intencion", out intent);

                ActivitiesByCourseViewModel actividad;
                context.UserData.TryGetValue("Actividad", out actividad);

                if (solicitud != null)
                {
                    long? intentId = null;

                    if (intent != null)
                        intentId = intent.IdIntencion;

                    var cursoManager = container.GetInstance<ICurso>();

                    List<CourseByModuleViewModel> studentActiveCourses = cursoManager.GetCourseByModuleActive(solicitud.IdAlumno);

                    CourseByModuleViewModel docenteCurso = studentActiveCourses.Where(s => s.Curso == curso).FirstOrDefault();

                    var respuestaPersonalizada = context.PrivateConversationData.GetValueOrDefault("custom", string.Empty);
                    var solucion = respuestaPersonalizada.Equals(string.Empty) ? receivedResult?.Speech : respuestaPersonalizada;

                    //await context.PostAsync(solucion);

                    int? idActividad = actividad?.IdActividad;
                    int? idEmpleado = docenteCurso?.IdEmpleado;
                    int? cumpleSLA = 2;
                    var tipoConsulta = context.UserData.GetValue<string>("tipo-consulta");

                    var dif = ConvertidorUtil.GmtToPacific(DateTime.Now).Subtract(solicitud.FechaRegistro).TotalHours;

                    if (tipoConsulta == "1" && dif <= 48)
                    {
                        cumpleSLA = 1;
                    }
                    else
                    {
                        if (dif <= 24)
                        {
                            cumpleSLA = 1;
                        }
                    }


                    bool IsEmailSent = false;

                    DateTime? fechaSolucion = null;

                    switch (estado)
                    {
                        case "A":
                            fechaSolucion = ConvertidorUtil.GmtToPacific(DateTime.Now);
                            break;

                        case "D":

                            solucion = string.Empty;
                            cumpleSLA = 0;

                            if (!string.IsNullOrEmpty(docenteCurso?.Email))
                            {

                                IsEmailSent = await SmtpEmailSender.SendEmailAsync("upc.chatbot@gmail.com",
                                    docenteCurso.Email,
                                    "UPECITO - Consultas Académicas No Resueltas",
                                    EmailTeacher(sesion.CodigoAlumno, sesion.NombreApePaterno, solicitud.Consulta));
                            }

                            if (IsEmailSent)
                            {
                                await context.PostAsync($"Su consulta ha sido derivada al Docente: {docenteCurso.Nombre} {docenteCurso.ApellidoPat}; quien le brindará una respuesta");
                            }
                            else
                            {
                                await context.PostAsync(receivedResult.Speech);

                            }

                            break;

                        default:
                            solucion = string.Empty;
                            cumpleSLA = 0;

                            break;
                    }

                    solicitudManager.Actualizar(solicitud.IdSolicitud, intentId, solucion, estado, userName, docenteCurso?.IdCurso, idActividad, idEmpleado, cumpleSLA, fechaSolucion);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}