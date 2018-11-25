using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FormBot.Util;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleInjector;
using Upecito.Bot.Upecito.Helpers;
using Upecito.Data.Interface;
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

            //var message = context.MakeMessage();
            //var attachment = GetInfoCard();
            //message.Attachments.Add(attachment);

            //await context.PostAsync(message);




            context.Wait(MessageReceivedAsync);
            return;




            //if (context == null)
            //{
            //    throw new ArgumentNullException(nameof(context));
            //}

            ////Set the Last Dialog in Conversation Data
            ////context.UserData.SetValue(Strings.LastDialogKey, Strings.LastDialogSend1on1Dialog);

            //var userId = context.Activity.From.Id;
            //var botId = context.Activity.Recipient.Id;
            //var botName = context.Activity.Recipient.Name;

            //var test = context.Activity.AsEventActivity();

            //var sesionData = context.UserData.GetValueOrDefault<Sesion>("sesion");

            //if (test?.Name == "requestRootDialog")
            //{
            //    //var channelData = context.Activity.GetChannelData<TeamsChannelData>();
            //    var connectorClient = new ConnectorClient(new Uri(context.Activity.ServiceUrl));

            //    var parameters = new ConversationParameters
            //    {
            //        Bot = new ChannelAccount(botId, botName),
            //        Members = new ChannelAccount[] { new ChannelAccount(userId) }
            //        //ChannelData = new TeamsChannelData
            //        //{
            //        //    Tenant = channelData.Tenant
            //        //}
            //    };

            //    var conversationResource = await connectorClient.Conversations.CreateConversationAsync(parameters);

            //    var message = Activity.CreateMessageActivity();
            //    message.From = new ChannelAccount(botId, botName);
            //    message.Conversation = new ConversationAccount(id: conversationResource.Id.ToString());
            //    message.Text = "Iniciar";

            //    await connectorClient.Conversations.SendToConversationAsync((Activity)message);

            //    context.Done<object>(null);
            //}
            //else
            //{
            //    context.Wait(MessageReceivedAsync);
            //}






            //var activity = context.Activity.AsMessageActivity();

            //if (!string.IsNullOrEmpty(context.Activity.From.Name))
            //{
            //    await context.PostAsync("Iniciar");
            //    //await Conversation.SendAsync(activity, () => new RootDialog());
            //}
            //else
            //{
            //    context.Wait(MessageReceivedAsync);
            //}


            //if (!string.IsNullOrEmpty(context.Activity.From.Name))
            //{
            //    string userId = context.Activity.From.Id;

            //    if (string.IsNullOrEmpty(userId))
            //    {
            //        context.Wait(MessageReceivedAsync);
            //        return;
            //    }

            //    var container = new Container();
            //    DependencyResolver.UnityConfig.RegisterTypes(container);

            //    var sesion = container.GetInstance<ISesion>();
            //    Sesion sesionData = sesion.CrearSesion(ConvertidorUtil.GetLong(userId));

            //    message = context.MakeMessage();
            //    message.Text = $"Hola {sesionData.Nombre}, soy UPECITO el asesor del Aula Virtual de UPC. Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";

            //    await context.PostAsync(message);

            //    Response inputLog = Helpers.PersistChatLog(null, sesionData, message.Text, "Bot", "BotFramework");

            //    if (sesionData != null)
            //    {
            //        context.UserData.SetValue("sesion", sesionData);
            //        context.Call(new MenuDialog(), EndWelcome);
            //    }
            //    else
            //    {
            //        context.Done(true);
            //    }
            //}

            //return;


            //context.Done(true);


            //context.Done(this);

            //context.Call(new WelcomeDialog(), EndWelcome);

            //context.Wait(ShowStartButton);

            //return;

            //context.Wait(MessageReceivedAsync);

            //PromptDialog.Choice(
            //    context: context,
            //    resume: ChoiceReceivedAsync,
            //    options: (IEnumerable<StartOptions>)Enum.GetValues(typeof(StartOptions)),
            //    prompt: "Presiona el botón para iniciar"
            //);

            //context.Done(true);
            //return;
        }

        public enum StartOptions
        {
            Iniciar
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            //var message = context.MakeMessage();
            //var attachment = GetInfoCard();
            //message.Attachments.Add(attachment);

            //await context.PostAsync(message);


            var result = context.Activity.AsMessageActivity();
            string userId = context.Activity.From.Id;

            //if (result?.Text == "Iniciar")
            //{
            //    Console.WriteLine("hola");
            //}

            var test = context.Activity.AsEventActivity();

            //var container = new Container();
            //DependencyResolver.UnityConfig.RegisterTypes(container);

            //var sesion = container.GetInstance<ISesion>();
            //var sesionData = sesion.CrearSesion(ConvertidorUtil.GetLong(userId));

            var sesionData = context.UserData.GetValueOrDefault<Sesion>("sesion");

            if (test?.Name == "requestRootDialog" || result?.Text.ToUpper() == "INICIAR")
            {
                //var p = new
                //{
                //    RequiresBotState = true,
                //    SupportsTts = true,
                //    SupportsListening = true
                //};

                //Entity entity = new Entity
                //{
                //    Type = "ClientCapabilities",
                //    Properties = JObject.FromObject(p)
                //};

                //context.Activity.Type = "message";
                //context.Activity.ChannelData = new { Paris = "Hola" };

                //context.Activity.Entities.Add(entity);

                //context.ConversationData.

                //await context.PostAsync("Iniciar");

                //var message = context.MakeMessage();
                //var attachment = RootDialog.GetInfoCard();
                //message.Attachments.Add(attachment);

                //await context.PostAsync(message);

                var message = context.MakeMessage();
                message.Text = $"Hola {sesionData.Nombre}, soy UPECITO el asesor del Aula Virtual de UPC. Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";

                await context.PostAsync(message);

                Response inputLog = Helpers.PersistChatLog(null, sesionData, message.Text, "Bot", "BotFramework");

            }

            context.UserData.SetValue("sesion", sesionData);
            context.Call(new MenuDialog(), EndWelcome);

            //if (sesionData != null)
            //{
            //    context.Call(new WelcomeDialog(), EndWelcome);
            //}
            //else
            //{
            //    //context.Wait(MessageReceivedAsync);
            //    context.Done(true);
            //}

            return;

            //var result = await activity;

            //if (string.IsNullOrEmpty(result.Text))
            //{
            //    //context.Done(this);

            //    //context.Wait(MessageReceivedAsync);
            //    //return;

            //    var message = context.MakeMessage();
            //    message.Text = "Iniciar";

            //    await context.PostAsync(message);
            //    await context.Forward(new WelcomeDialog(), EndWelcome, result, CancellationToken.None);
            //}
            //else
            //{
            //    context.Call(new WelcomeDialog(), EndWelcome);
            //}

            //await context.Forward(new WelcomeDialog(), EndWelcome, result, CancellationToken.None);


            //context.Call(new WelcomeDialog(), EndWelcome);

            //if (result.Text == "Iniciar")
            //{
            //    //await context.Forward(new WelcomeDialog(), EndWelcome, result, CancellationToken.None);
            //    ////context.Call(new WelcomeDialog(), EndWelcome);
            //    //return;

            //    // context.Activity.Entities.Add(new Entity { Type= });


            //    await context.PostAsync(result.Text);

            //    string userId = context.Activity.From.Id;

            //    if (string.IsNullOrEmpty(userId))
            //    {
            //        context.Wait(MessageReceivedAsync);
            //        //return;
            //    }


            //    var message = context.MakeMessage();
            //    var attachment = GetInfoCard();
            //    message.Attachments.Add(attachment);

            //    await context.PostAsync(message);

            //    var container = new Container();
            //    DependencyResolver.UnityConfig.RegisterTypes(container);

            //    var sesion = container.GetInstance<ISesion>();
            //    Sesion sesionData = sesion.CrearSesion(ConvertidorUtil.GetLong(userId));

            //    message = context.MakeMessage();
            //    message.Text = $"Hola {sesionData.Nombre}, soy UPECITO el asesor del Aula Virtual de UPC. Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";

            //    await context.PostAsync(message);

            //    Response inputLog = Helpers.PersistChatLog(null, sesionData, message.Text, "Bot", "BotFramework");

            //    if (sesionData != null)
            //    {
            //        context.UserData.SetValue("sesion", sesionData);
            //        context.Call(new MenuDialog(), EndWelcome);
            //    }
            //    else
            //    {
            //        context.Done(true);
            //    }
            //}
            //else
            //{

            //    context.Call(new MenuDialog(), EndWelcome);
            //}

        }

        public virtual async Task ShowStartButton(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            //context.Call(new WelcomeDialog(), EndWelcome);

            var result = await activity;

            var message = context.MakeMessage();
            var attachment = GetInfoCard();
            message.Attachments.Add(attachment);

            await context.PostAsync(message);

            //await context.Forward(new WelcomeDialog(), EndWelcome, result, CancellationToken.None);
            context.Call(new WelcomeDialog(), EndWelcome);

            //if (string.IsNullOrEmpty(message.Text))
            //{
            //    context.Wait(ShowStartButton);
            //}
            //else
            //{
            //    await context.Forward(new WelcomeDialog(), EndWelcome, message, CancellationToken.None);
            //}


            //PromptDialog.Choice(
            //    context: context,
            //    resume: ChoiceReceivedAsync,
            //    options: (IEnumerable<StartOptions>)Enum.GetValues(typeof(StartOptions)),
            //    prompt: "Presiona el botón para iniciar",
            //    retry: "Por favor intenta de nuevo"
            //);

            //var sesionData = context.UserData.GetValueOrDefault<Sesion>("sesion");

            //object channelData = context.Activity.ChannelData;

            //if (channelData == null)
            //{
            //    context.Done(true);
            //}
            //else
            //{
            //    context.Call(new WelcomeDialog(), EndWelcome);

            //}

            ////if (sesionData != null)
            ////    context.Call(new WelcomeDialog(), EndWelcome);
            ////else
            ////    context.Done(true);

            //return Task.CompletedTask;
        }

        public virtual async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<StartOptions> activity)
        {
            context.Call<object>(new WelcomeDialog(), ChildDialogComplete);
        }

        public virtual async Task ChildDialogComplete(IDialogContext context, IAwaitable<object> response)
        {
            var sesionData = context.UserData.GetValueOrDefault<ISesionData>("sesion");

            if (sesionData != null)
                context.Done(true);
            else
                context.Call(new WelcomeDialog(), EndWelcome);
        }

        public virtual async Task EndWelcome(IDialogContext context, IAwaitable<object> response)
        {
            context.Done(this);
        }

        public static Attachment GetInfoCard()
        {
            var infoCard = new HeroCard
            {
                Title = "Asesor del Aula Virtual ",
                Images = new List<CardImage> { new CardImage("https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/UPC_logo_transparente.png/240px-UPC_logo_transparente.png") }
            };

            return infoCard.ToAttachment();
        }
    }
}