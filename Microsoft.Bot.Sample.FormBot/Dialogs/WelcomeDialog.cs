﻿using System;
using System.Threading.Tasks;
using FormBot.Util;
using Microsoft.Bot.Builder.Dialogs;
using SimpleInjector;
using Upecito.Business;
using Upecito.Data.Implementation;
using Upecito.Data.Interface;
using Upecito.Interface;

namespace FormBot.Dialogs
{
    [Serializable]
    public class WelcomeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //var userName = "U201502689"; 
            var userName = context.Activity.From.Name;

            //var userId = "1";
            var userId = context.Activity.From.Id;

            var container = new Container();
            DependencyResolver.UnityConfig.RegisterTypes(container);

            var sesion = container.GetInstance<ISesion>();
            var sesionData = sesion.CrearSesion(ConvertidorUtil.GetLong(userId));
            context.UserData.SetValue("sesion", sesionData);

            if (sesionData == null)
            {
                context.Done(true);
                return;
            }

            var message = context.MakeMessage();
            message.Text = $"Hola {sesionData.NombreApePaterno}, soy UPECITO el asesor del Aula Virtual de UPC.Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";
            await context.PostAsync(message);

            context.Call(new MenuDialog(), ResumeWelcome);
        }

        private async Task ResumeWelcome(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(true);
        }
    }
}