using FormBot.DependencyResolver;
using FormBot.Util;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleInjector;
using System;
using System.Threading.Tasks;
using Upecito.Bot.Upecito.Helpers;
using Upecito.Interface;
using Upecito.Model;
using Upecito.Model.Common;

namespace FormBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return;
        }

        public enum StartOptions
        {
            Iniciar
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string userId = context.Activity.From.Id;

            var messageActivity = context.Activity.AsMessageActivity();
            var eventActivity = context.Activity.AsEventActivity();


            var sesionData = context.UserData.GetValueOrDefault<Sesion>("sesion");

            if (sesionData == null)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(context.Activity.ServiceUrl));

                var container = new Container();
                UnityConfig.RegisterTypes(container);

                var sesion = container.GetInstance<ISesion>();
                sesionData = sesion.CrearSesion(ConvertidorUtil.GetLong(userId));
                context.UserData.SetValue("sesion", sesionData);

            }

            if (eventActivity?.Name == "requestRootDialog" || messageActivity?.Text.ToUpper() == "INICIAR")
            {
                var message = context.MakeMessage();
                message.Text = $"Hola {sesionData.Nombre}, soy UPECITO el asesor del Aula Virtual de UPC. Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";

                await context.PostAsync(message);

                Response inputLog = Helpers.PersistChatLog(null, sesionData, message.Text, "Bot", "BotFramework");
            }


            context.Call(new MenuDialog(), EndWelcome);
            return;

       }

        public virtual async Task EndWelcome(IDialogContext context, IAwaitable<object> response)
        {
            context.Done(this);
        }

    }
}