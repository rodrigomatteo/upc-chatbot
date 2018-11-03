﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using SimpleInjector;
using Upecito.Business;
using Upecito.Data.Implementation;
using Upecito.Data.Interface;
using Upecito.Interface;
using FormBot.Util;
using Microsoft.Bot.Connector;

namespace FormBot.Dialogs
{
    [Serializable]
    public class WelcomeDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            try
            {
                var userName = context.Activity.From.Name;
                var userId = context.Activity.From.Id;

                if (!string.IsNullOrEmpty(userName))
                {
                    var message = context.MakeMessage();
                    message.Text = $"Hola {userName}, soy UPECITO el asesor del Aula Virtual de UPC.Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";

                    context.PostAsync(message);

                    var container = new Container();
                    DependencyResolver.UnityConfig.RegisterTypes(container);

                    var sesion = container.GetInstance<ISesion>();

                    var sesionData = sesion.CrearSesion(ConvertidorUtil.GetLong(userId));

                    if (sesionData == null)
                    {
                        var message_ = context.MakeMessage();
                        message.Text = "No se pudo realizar la conexión con el servidor";
                        context.PostAsync(message_);

                        context.Wait(this.MessageReceived);
                    }
                    else
                    {
                        context.UserData.SetValue("sesion", sesionData);
                        context.Call(new MenuDialog(), ResumeWelcome);
                    }                   
                }                
            }
            catch(Exception ex)
            {

            }
            return Task.CompletedTask;
        }

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

        private async Task ResumeWelcome(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(true);
        }
    }
}