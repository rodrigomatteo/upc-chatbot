using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using SimpleInjector;
using Upecito.Business;
using Upecito.Data.Implementation;
using Upecito.Data.Interface;
using Upecito.Interface;
using FormBot.Util;

namespace FormBot.Dialogs
{
    [Serializable]
    public class WelcomeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var userName = context.Activity.From.Name;
            var userId = context.Activity.From.Id;

            var message = context.MakeMessage();
            message.Text = $"Hola {userName}, soy UPECITO el asesor del Aula Virtual de UPC.Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";

            await context.PostAsync(message);

            var container = new Container();
            DependencyResolver.UnityConfig.RegisterTypes(container);

            var sesion = container.GetInstance<ISesion>();

            var sesionData = sesion.CrearSesion(ConvertidorUtil.GetLong(userId));

            if (sesionData == null)
            {
                context.Done(true);
                return;
            }

            context.UserData.SetValue("sesion", sesionData);
            context.Call(new MenuDialog(), ResumeWelcome);
        }

        private async Task ResumeWelcome(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(true);
        }
    }
}