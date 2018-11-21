using FormBot.Util;
using Microsoft.Bot.Builder.Dialogs;
using SimpleInjector;
using System;
using System.Threading.Tasks;
using Upecito.Bot.Upecito.Helpers;
using Upecito.Interface;
using Upecito.Model;

namespace FormBot.Dialogs
{
    [Serializable]
    public class WelcomeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            string userId = context.Activity.From.Id;
            //string userId = "4";

            if (string.IsNullOrEmpty(userId))
            {
                context.Done(true);
                return;
            }

            var container = new Container();
            DependencyResolver.UnityConfig.RegisterTypes(container);

            var sesion = container.GetInstance<ISesion>();
            Sesion sesionData = sesion.CrearSesion(ConvertidorUtil.GetLong(userId));

            var message = context.MakeMessage();
            message.Text = $"Hola {sesionData.Nombre}, soy UPECITO el asesor del Aula Virtual de UPC.Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";

            await context.PostAsync(message);

            Helpers.PersistChatLog(null, sesionData, message.Text, "Bot", "BotFramework");

            if (sesionData != null)
            {
                context.UserData.SetValue("sesion", sesionData);
                context.Call(new MenuDialog(), ResumeWelcome);
            }
            else
            {
                context.Done(true);
            }

        }

        private async Task ResumeWelcome(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(true);
        }
    }
}