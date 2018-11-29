using FormBot.Util;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleInjector;
using System;
using System.Threading;
using System.Threading.Tasks;
using Upecito.Bot.Upecito.Helpers;
using Upecito.Interface;
using Upecito.Model;
using Upecito.Model.Common;

namespace FormBot.Dialogs
{
    [Serializable]
    public class WelcomeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //context.Wait(MessageReceivedAsync);

            string userId = context.Activity.From.Id;
            ////////object channelData = context.Activity.ChannelData;

            ////TODO: Manual override for testing purposes
            //userId = "1";

            ////////if (string.IsNullOrEmpty(userId))
            ////////{
            ////////    //context.Wait(MessageReceivedAsync);
            ////////    //context.Done(true);
            ////////    return;
            ////////}

            var activity = context.Activity.AsMessageActivity();

            //if (string.IsNullOrEmpty(activity?.Text))
            //{
            //    context.Done(true);
            //    return;
            //}

            var test = context.Activity.AsEventActivity();


            if (test?.Name == "requestRootDialog")
            {
                //var container = new Container();
                //DependencyResolver.UnityConfig.RegisterTypes(container);

                //var sesion = container.GetInstance<ISesion>();
                //Sesion sesionData = sesion.CrearSesion(ConvertidorUtil.GetLong(userId));

                Sesion sesionData = context.UserData.GetValueOrDefault<Sesion>("sesion");

                var message = context.MakeMessage();
                //var attachment = RootDialog.GetInfoCard();
                //message.Attachments.Add(attachment);

                //await context.PostAsync(message);

                message = context.MakeMessage();
                message.Text = $"Hola {sesionData.Nombre}, soy UPECITO el asesor del Aula Virtual de UPC. Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";

                await context.PostAsync(message);

                Response inputLog = Helpers.PersistChatLog(null, sesionData, message.Text, "Bot", "BotFramework");

                if (sesionData != null)
                {
                    context.UserData.SetValue("sesion", sesionData);
                    context.Call(new MenuDialog(), ResumeWelcome);
                    //context.Forward(new MenuDialog(), ResumeWelcome, message, CancellationToken.None).Wait();
                }
                else
                {
                    context.Done(true);
                }
            }
            else
            {
                context.Call(new MenuDialog(), ResumeWelcome);
            }

            //}
            //else
            //{
            //    context.Done(this);
            //    //context.Wait(MessageReceivedAsync);
            //    //context.Call(new MenuDialog(), ResumeWelcome);
            //}

            //return Task.CompletedTask;
            return;
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            //context.Call(new MenuDialog(), ResumeWelcome);

            string userId = context.Activity.From.Id;
            var result = await activity;

            //TODO: Manual override for testing purposes
            //userId = "1";

            //if (string.IsNullOrEmpty(userId))
            //{
            //    context.Wait(MessageReceivedAsync);
            //    //context.Done(true);
            //    return;
            //}

            //if (string.IsNullOrEmpty(result.Text))
            //{
            //    //context.Done(true);
            //    //context.Wait(MessageReceivedAsync);
            //    //return;
            //    context.Call(new RootDialog(), null);
            //}

            if (result.Text == "Iniciar")
            {
                var container = new Container();
                DependencyResolver.UnityConfig.RegisterTypes(container);

                var sesion = container.GetInstance<ISesion>();
                Sesion sesionData = sesion.CrearSesion(ConvertidorUtil.GetLong(userId));

                var message = context.MakeMessage();
                //var attachment = RootDialog.GetInfoCard();
                //message.Attachments.Add(attachment);

                //await context.PostAsync(message);

                message = context.MakeMessage();
                message.Text = $"Hola {sesionData.Nombre}, soy UPECITO el asesor del Aula Virtual de UPC. Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";

                await context.PostAsync(message);

                Response inputLog = Helpers.PersistChatLog(null, sesionData, message.Text, "Bot", "BotFramework");

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
            else
            {
                //context.Wait(MessageReceivedAsync);
                context.Call(new MenuDialog(), ResumeWelcome);
            }

            //await context.Forward(new MenuDialog(), ResumeWelcome, result, CancellationToken.None);


        }

        private async Task ResumeWelcome(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(true);
        }
    }
}