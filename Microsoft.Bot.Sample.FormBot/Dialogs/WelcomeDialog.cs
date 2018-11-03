using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using SimpleInjector;
using Upecito.Business;
using Upecito.Data.Implementation;
using Upecito.Data.Interface;
using Upecito.Interface;
using FormBot.Util;
using Microsoft.Bot.Connector;
using Upecito.Model;

namespace FormBot.Dialogs
{
    [Serializable]
    public class WelcomeDialog : IDialog<object>
    {
        private enum Selection
        {
            Academic, Technical
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        //public Task StartAsync(IDialogContext context)
        //{
        //    try
        //    {
        //        var userName = context.Activity.From.Name;
        //        var userId = context.Activity.From.Id;

        //        if (!string.IsNullOrEmpty(userName))
        //        {
        //            var message = context.MakeMessage();
        //            message.Text = $"Hola {userName}, soy UPECITO el asesor del Aula Virtual de UPC.Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";

        //            context.PostAsync(message);

        //            var container = new Container();
        //            DependencyResolver.UnityConfig.RegisterTypes(container);

        //            var sesion = container.GetInstance<ISesion>();

        //            var sesionData = sesion.CrearSesion(ConvertidorUtil.GetLong(userId));

        //            if (sesionData == null)
        //            {
        //                var message_ = context.MakeMessage();
        //                message.Text = "No se pudo realizar la conexión con el servidor";
        //                context.PostAsync(message_);

        //                context.Wait(this.MessageReceived);
        //            }
        //            else
        //            {
        //                context.UserData.SetValue("sesion", sesionData);
        //                context.Call(new MenuDialog(), ResumeWelcome);
        //            }                   
        //        }                
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //    return Task.CompletedTask;
        //}

        private async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if ((message.Text != null) && (message.Text.Trim().Length > 0))
            {
                context.Done<object>(null);
            }
            else
            {
                context.Fail(new Exception("Message was not a string or was an empty string."));
            }
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var userName = context.Activity.From.Name;
            var userId = context.Activity.From.Id;

            //if (!string.IsNullOrEmpty(userName))
            //{
            //    var message = context.MakeMessage();
            //    message.Text = $"Hola {userName}, soy UPECITO el asesor del Aula Virtual de UPC.Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";

            //    await context.PostAsync(message);

            //    var container = new Container();
            //    DependencyResolver.UnityConfig.RegisterTypes(container);

            //    var sesion = container.GetInstance<ISesion>();

            //    var sesionData = sesion.CrearSesion(ConvertidorUtil.GetLong(userId));

            //    if (sesionData == null)
            //    {
            //        var message_ = context.MakeMessage();
            //        message.Text = "No se pudo realizar la conexión con el servidor";
            //        await context.PostAsync(message_);

            //        context.Wait(this.MessageReceived);
            //    }
            //    else
            //    {
            //        context.UserData.SetValue("sesion", sesionData);
            //        context.Call(new MenuDialog(), ResumeWelcome);
            //    }
            //}

            var activity = await result as Activity;

            var message = context.MakeMessage();
            var strMessage = $"Hola {userName}, soy UPECITO el asesor del Aula Virtual de UPC.Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";
            
            var initialText = activity.Text.ToLower();

            string[] choices = new string[] { "Consultas Académicas", "Consultas y Problemas Técnicos" };

            await context.PostAsync(strMessage);
            
            if (initialText == "start-upecito-bot")
            {
                var options = new[] { Selection.Academic, Selection.Technical };
                var descriptions = new[] { "Consultas Académicas", "Consultas y Problemas Técnicos" };

                PromptDialog.Choice<Selection>(context, OnOptionSelected, options, "Selecciona el canal de atención en el que requieres ayuda", descriptions: descriptions);               
            }
            else
            {
                await context.PostAsync($"You sent {activity.Text} which was characters.");
                context.Wait(MessageReceivedAsync);
            }

        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<Selection> result)
        {
            var optionSelected = await result;

            switch (optionSelected)
            {
                case Selection.Academic:
                    var userName = context.Activity.From.Name;
                    PromptDialog.Text(context, ResumeGetAcademicIntent, $"Por favor {userName}, dime tu consulta sobre Consultas Académicas", "Intenta de nuevo");
                    break;
                case Selection.Technical:
                    await context.PostAsync("Seleccionaste consulta tecnica");
                    ShowPrompt(context);
                    break;
            }
        }

        private void ShowPrompt(IDialogContext context)
        {
            var options = new[] { Selection.Academic, Selection.Technical };
            var descriptions = new[] { "Consultas Académicas", "Consultas y Problemas Técnicos" };

            PromptDialog.Choice<Selection>(context, OnOptionSelected, options, "Selecciona el canal de atención en el que requieres ayuda", descriptions: descriptions);

        }

        private async Task ResumeGetAcademicIntent(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await Process(context);
        }

        private async Task ResumeGetAcademicIntent(IDialogContext context, IAwaitable<string> result)
        {
            await Process(context);
        }

        private async Task resumeAfterPrompt(IDialogContext context, IAwaitable<string> result)
        {
            string choice = await result;

            await context.PostAsync($"You selected {choice}");
        }

        private async Task ResumeWelcome(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(true);
        }


        private async Task Process(IDialogContext context)
        {
            var activity = context.Activity as Activity;

            //var userId = Convert.ToInt32(context.Activity.From.Id);
            var userId = context.UserData.GetValue<Sesion>("sesion").IdAlumno;

            /*
             * 4.1.4   El Sistema crea una nueva Solicitud Académica con los datos indicados líneas abajo
             * en la entidad[GSAV_SolicitudAcadémica], generando un código único
            */
            var container = new Container();
            DependencyResolver.UnityConfig.RegisterTypes(container);


            var solicitudManager = container.GetInstance<ISolicitud>();
            var solicitud = solicitudManager.CrearSolicitud(1, userId, null, activity.Text, "");
            context.UserData.SetValue("solicitud", solicitud);

            var handler = container.GetInstance<DialogEngine>();
            var receivedResult = await handler.GetSpeechAsync(activity);

            var intencionManager = container.GetInstance<IIntencion>();
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
                if (receivedResult.ExistValidIntent)
                {
                    var intent = receivedResult.GetValidIntent();
                    var intencion = intencionManager.ObtenerCategoria(intent);

                    context.UserData.SetValue<Result>("result", receivedResult);

                    if (intencion != null && intencion.Nombre != string.Empty)
                    {
                        switch (intencion.Nombre)
                        {
                            /*
                             * 4.1.7	Si la “Intención de Consulta” es “Programación de Actividades”, 
                             * el sistema extiende el caso de uso: GSAV_CUS005_Consultar Programación de Actividades
                            */
                            case AppConstant.Intencion.PROGRAMACION:
                                context.Call(new ProgramacionActividadesDialog(), ResumeAfterSuccessAcademicIntent);
                                break;
                            /*
                             * 4.1.8	Si la “Intención de Consulta” es “Calendario Académico”, 
                             * el sistema extiende el caso de uso: GSAV_CUS006_Consultar Calendario Académico
                            */
                            case AppConstant.Intencion.CALENDARIO:
                                context.Call(new CalendarioDialog(), ResumeAfterSuccessAcademicIntent);
                                break;
                            case AppConstant.Intencion.ORGANIZACION:
                                context.Call(new OrganizacionDialog(), ResumeAfterSuccessAcademicIntent);
                                break;
                            /*
                            * 4.1.9	Si la “Intención de Consulta” es “Organización de Aula Virtual”, “Matricula”, 
                            * “Reglamento de Asistencia”, “Retiro del Curso”, “Promedio Ponderado”, el sistema extiende el caso de uso: 
                            * GSAV_CUS007_Consultar Temas Frecuentes
                           */
                            case AppConstant.Intencion.MATRICULA:
                            case AppConstant.Intencion.ASISTENCIA:
                            case AppConstant.Intencion.RETIRO:
                            case AppConstant.Intencion.PROMEDIO:
                                context.Call(new PreguntasFrecuentesDialog(), ResumeAfterSuccessAcademicIntent);
                                break;
                            /*
                             * 4.1.10  Si la “Intención de Consulta” es “Créditos de un Curso”, el sistema extiende el caso de uso: 
                             * GSAV_CUS008_Consultar Créditos de un Curso
                            */
                            case AppConstant.Intencion.CREDITOS:
                                context.Call(new CreditosDialog(), ResumeAfterSuccessAcademicIntent);
                                break;
                            case AppConstant.Intencion.DEFAULT:
                                context.Call(new EntrenandoDialog(), ResumeAfterUnknownAcademicIntent);
                                break;
                            default:
                                /*
                                 * Si en el punto [4.1.3] el sistema corrobora que no existe una repuesta
                                 * para el tipo de consulta ingresada por el alumno, entonces deriva la
                                 * consulta al docente enviando un correo electrónico y actualiza el estado
                                 * de la solicitud académica [GSAV_RN014-Estado de la Solicitud],
                                 * [GSAV_RN004-Comsultas Académicas No Resueltas]
                                 */
                                context.Call(new NoRespuestaDialog(), ResumeAfterUnknownAcademicIntent);
                                break;
                        }
                    }
                    else
                    {
                        var userName = context.Activity.From.Name;
                        var message = context.MakeMessage();
                        message.Text = $"{userName}, no he podido registrar tu solicitud o la intención no se ha encontrado";

                        await context.PostAsync(message);
                        context.Wait(ResumeGetAcademicIntent);
                    }
                }
                else
                {
                    /*
                     * Si en el punto [4.1.3] el sistema corrobora que no existe una repuesta
                     * para el tipo de consulta ingresada por el alumno, entonces deriva la
                     * consulta al docente enviando un correo electrónico y actualiza el estado
                     * de la solicitud académica [GSAV_RN014-Estado de la Solicitud],
                     * [GSAV_RN004-Comsultas Académicas No Resueltas]
                     */
                    context.Call(new SinScoreDialog(), ResumeAfterUnknownAcademicIntent);
                }
            }
            else
                context.Call(new SinScoreDialog(), ResumeAfterUnknownAcademicIntent);
        }

        private async Task ResumeAfterSuccessAcademicIntent(IDialogContext context, IAwaitable<object> result)
        {
            var container = new Container();
            DependencyResolver.UnityConfig.RegisterTypes(container);


            var solicitudManager = container.GetInstance<ISolicitud>();

            /*
             * 4.1.11	El sistema valida si obtuvo respuesta [GSAV_SolicitudAcadémica] -- Actualiza la solicitud creada con la respuesta obtenida
             */
            /*
             * 4.1.12	El sistema actualiza el estado de la Solicitud Académica a “Atendida” [GSAV_RN014-Estado de la Consulta]
             */
            var solicitud = context.UserData.GetValue<Solicitud>("solicitud");
            var userName = context.Activity.From.Name;
            Result receivedResult;
            context.UserData.TryGetValue<Result>("result", out receivedResult);
            solicitudManager.Actualizar(solicitud.IdSolicitud, null, string.Empty, AppConstant.EstadoSolicitud.ATENDIDO, userName);

            // 4.1.14  El caso de uso finaliza
            context.Wait(MessageReceivedAsync);
        }

        private async Task ResumeAfterUnknownAcademicIntent(IDialogContext context, IAwaitable<object> result)
        {
            var container = new Container();
            DependencyResolver.UnityConfig.RegisterTypes(container);

            var solicitudManager = container.GetInstance<ISolicitud>();

            /*
             * 4.1.11	El sistema valida si obtuvo respuesta [GSAV_SolicitudAcadémica] -- Actualiza la solicitud creada con la respuesta obtenida
             */
            /*
             * 4.1.12	El sistema actualiza el estado de la Solicitud Académica a “Atendida” [GSAV_RN014-Estado de la Consulta]
             */
            var solicitud = context.UserData.GetValueOrDefault<Solicitud>("solicitud");
            var userName = context.Activity.From.Name;
            var receivedResult = context.UserData.GetValueOrDefault<Result>("result");

            if (solicitud != null && receivedResult != null)
                solicitudManager.Actualizar(solicitud.IdSolicitud, null, string.Empty, AppConstant.EstadoSolicitud.INVALIDO, userName);

            // 4.1.14  El caso de uso finaliza
            await Task.Delay(2000);
            ShowPrompt(context);
        }

    }
}