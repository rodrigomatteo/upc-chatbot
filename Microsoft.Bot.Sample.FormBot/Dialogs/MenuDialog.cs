﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Api.Ai.ApplicationService.Factories;
using Api.Ai.Domain.Service.Factories;
using Api.Ai.Infrastructure.Factories;
using SimpleInjector;
using Upecito.Interface;
using Upecito.Business;
using Upecito.Data.Implementation;
using Upecito.Data.Interface;
using Upecito.Model;
using FormBot.Dialogflow;
using Newtonsoft.Json;
using System.Collections.Generic;
using FormBot.Services;
using Upecito.Bot.Upecito.Helpers;

namespace FormBot.Dialogs
{
    [Serializable]
    public class MenuDialog : IDialog<object>
    {
        private enum Selection
        {
            Consultas_Academicas, Consultas_Tecnicas
        }

        public Task StartAsync(IDialogContext context)
        {
            var options = new[] { Selection.Consultas_Academicas, Selection.Consultas_Tecnicas };
            var descriptions = new[] { "Consultas Académicas", "Consultas y Problemas Técnicos" };

            PromptDialog.Choice(
               context: context,
               resume: OnOptionSelected,
               options: options,
               descriptions: descriptions,
               prompt: "Selecciona el canal de atención en el que requieres ayuda"
               //retry: "Por favor intenta de nuevo"
           );

            return Task.CompletedTask;
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<Selection> result)
        {
            var optionSelected = await result;
            var userName = context.UserData.GetValue<Sesion>("sesion").NombreApePaterno;

            switch (optionSelected)
            {
                case Selection.Consultas_Academicas:
                    context.UserData.SetValue("tipo-consulta", AppConstant.CanalAtencion.ConsultasAcademicas);                    
                    PromptDialog.Text(context, ResumeGetAcademicIntent, $"Por favor {userName}, dime tu consulta sobre Consultas Académicas", "Intenta de nuevo");
                    break;
                case Selection.Consultas_Tecnicas:
                    context.UserData.SetValue("tipo-consulta", AppConstant.CanalAtencion.ConsultasTecnicas);                   
                    PromptDialog.Text(context, ResumeGetAcademicIntent, $"Por favor {userName}, dime tu consulta sobre Consultas Ténicas", "Intenta de nuevo");
                    break;
            }

        }

        private async Task ResumeGetAcademicIntent(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await Process(context);
        }

        private async Task ResumeGetAcademicIntent(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                await Process(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task Process(IDialogContext context)
        {
            var activity = context.Activity as Activity;

            Sesion sesion = context.UserData.GetValue<Sesion>("sesion");

            var userId = sesion.IdAlumno;
            var codigoAlumno = sesion.CodigoAlumno;
            var idSesion = sesion.IdSesion;

            var tipoConsulta = context.UserData.GetValue<string>("tipo-consulta");

            /*
             * 4.1.4   El Sistema crea una nueva Solicitud Académica con los datos indicados líneas abajo
             * en la entidad[GSAV_SolicitudAcadémica], generando un código único
            */
            var container = new Container();
            DependencyResolver.UnityConfig.RegisterTypes(container);
           
            var solicitudManager = container.GetInstance<ISolicitud>();
            Solicitud solicitud = solicitudManager.CrearSolicitud(Convert.ToInt32(tipoConsulta), userId, null, idSesion, activity.Text, codigoAlumno);

            context.UserData.SetValue("solicitud", solicitud);

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
                    if (receivedResult.ExistValidIntent)
                    {
                        string intent = receivedResult.GetValidIntent();
                        Intencion intencion = intencionManager.ObtenerCategoria(intent);

                        context.UserData.SetValue("result", receivedResult);

                        if (string.IsNullOrEmpty(intencion.Nombre))
                        {
                            intencion.Nombre = intent;
                        }

                        context.UserData.SetValue("intencion", intencion);

                        if (!string.IsNullOrEmpty(intencion.Nombre))
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
                                case AppConstant.Intencion.CREDITOS:
                                case AppConstant.Intencion.ORGANIZACION:
                                case AppConstant.Intencion.CALENDARIO:
                                    //context.Call(new CalendarioDialog(), ResumeAfterSuccessAcademicIntent);
                                    var message = context.MakeMessage();
                                    message.Text = $"Esta es una consulta de: {intencion.Nombre}";
                                    await context.PostAsync(message);
                                    context.Wait(MessageReceivedAsync);
                                    break;

                                //case AppConstant.Intencion.ORGANIZACION:
                                //    context.Call(new OrganizacionDialog(), ResumeAfterSuccessAcademicIntent);
                                //    break;
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
                                //case AppConstant.Intencion.CREDITOS:
                                //    context.Call(new CreditosDialog(), ResumeAfterSuccessAcademicIntent);
                                //    break;
                                case AppConstant.Intencion.DEFAULT:
                                    context.Call(new NoRespuestaDialog(), ResumeAfterFailedAcademicIntent);
                                    break;

                                case "Notas":
                                    string dfParams = receivedResult.Intents[0].Parameters;
                                    string dfContext = receivedResult.OutputContexts;

                                    Dictionary<string, string> listParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(dfParams);

                                    string course = listParams["course"];
                                    string assignment = listParams["assignment"];

                                    if (!string.IsNullOrEmpty(course))
                                    {
                                        context.UserData.SetValue("course", course);
                                    }

                                    if (!string.IsNullOrEmpty(assignment))
                                    {
                                        context.UserData.SetValue("assignment", assignment);
                                    }


                                    if (receivedResult.Intents[0].AllRequiredParamsPresent)
                                    {

                                    }


                                    await context.PostAsync(receivedResult.Speech);
                                    context.Wait(MessageReceivedAsync);
                                    break;
                                default:
                                    /*
                                     * Si en el punto [4.1.3] el sistema corrobora que no existe una repuesta
                                     * para el tipo de consulta ingresada por el alumno, entonces deriva la
                                     * consulta al docente enviando un correo electrónico y actualiza el estado
                                     * de la solicitud académica [GSAV_RN014-Estado de la Solicitud],
                                     * [GSAV_RN004-Comsultas Académicas No Resueltas]
                                     */
                                    context.Call(new NoRespuestaDialog(), ResumeAfterFailedAcademicIntent);
                                    break;
                            }
                        }
                        else
                        {
                            var userName = context.UserData.GetValue<Sesion>("sesion").Nombre;
                            var message = context.MakeMessage();
                            message.Text = $"Uhmmm... {userName} estoy entrenándome para ayudarte más adelante con este tipo de dudas. Pero recuerda que vía Contacto UPC:  http://www.upc.edu.pe/servicios/contacto-upc puedes resolver tus dudas o consultas.";

                            context.PrivateConversationData.SetValue("custom", message.Text);

                            await context.PostAsync(message);

                            await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.INVALIDO);

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private async Task ResumeAfterSuccessAcademicIntent(IDialogContext context, IAwaitable<object> result)
        {
            await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.ATENDIDO);
            // 4.1.14  El caso de uso finaliza
            context.Wait(MessageReceivedAsync);
        }

        private async Task ResumeAfterUnknownAcademicIntent(IDialogContext context, IAwaitable<object> result)
        {
            await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.INVALIDO);

            // 4.1.14  El caso de uso finaliza
            await Task.Delay(2000);
            //ShowPrompt(context);
        }

        private async Task ResumeAfterFailedAcademicIntent(IDialogContext context, IAwaitable<object> result)
        {
            await ActualizarSolicitud(context, AppConstant.EstadoSolicitud.FALTAINFORMACION);

            // 4.1.14  El caso de uso finaliza
            //await Task.Delay(2000);
            //ShowPrompt(context);

            context.Done(this);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            await ResumeGetAcademicIntent(context, new AwaitableFromItem<string>(""));
        }

        private async Task ActualizarSolicitud(IDialogContext context, string estado)
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
            Solicitud solicitud = context.UserData.GetValueOrDefault<Solicitud>("solicitud");
            Sesion sesion = context.UserData.GetValue<Sesion>("sesion");

            string userName = sesion.CodigoAlumno;

            Result receivedResult;
            context.UserData.TryGetValue("result", out receivedResult);

            Intencion intent;
            context.UserData.TryGetValue("intencion", out intent);


            if (solicitud != null && receivedResult != null)
            {
                long? intentId = null;

                if (intent != null)
                    intentId = intent.IdIntencion;

                var respuestaPersonalizada = context.PrivateConversationData.GetValueOrDefault("custom", string.Empty);
                var solucion = respuestaPersonalizada.Equals(string.Empty) ? receivedResult.Speech : respuestaPersonalizada;

                solicitudManager.Actualizar(solicitud.IdSolicitud, intentId, solucion, estado, userName);

                bool IsEmailSent = await SmtpEmailSender.SendEmailAsync("upc.chatbot@gmail.com", "parismiguel@gmail.com", "UPECITO - Email test", Helpers.EmailTeacher(int.Parse(sesion.CodigoAlumno), sesion.NombreApePaterno, solucion));

            }
        }
    }
}