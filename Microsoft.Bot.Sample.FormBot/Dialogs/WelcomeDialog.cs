using FormBot.Util;
using Microsoft.Bot.Builder.Dialogs;
using SimpleInjector;
using System;
using System.Threading.Tasks;
using Upecito.Interface;

namespace FormBot.Dialogs
{
    [Serializable]
    public class WelcomeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //TODO: var userId = context.Activity.From.Id;
            var userId = "4";

            var container = new Container();
            DependencyResolver.UnityConfig.RegisterTypes(container);

            var sesion = container.GetInstance<ISesion>();
            var sesionData = sesion.CrearSesion(ConvertidorUtil.GetLong(userId));

            var message = context.MakeMessage();
            message.Text = $"Hola {sesionData.Nombre}, soy UPECITO el asesor del Aula Virtual de UPC.Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";

            await context.PostAsync(message);

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