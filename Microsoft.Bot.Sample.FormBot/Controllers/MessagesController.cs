using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Web.Http.Description;
using System.Net.Http;
using System.Linq;
using System.Net;
using SimpleInjector;
using FormBot.Dialogs;
using FormBot;
using System;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Autofac;
using System.Diagnostics;
using FormBot.DependencyResolver;
using Upecito.Interface;
using FormBot.Util;
using Upecito.Model.Common;
using Upecito.Bot.Upecito.Helpers;

namespace Microsoft.Bot.Sample.FormBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        #region Private Fields

        private readonly Container container;

        #endregion

        #region Constructor

        public MessagesController()
        {

        }

        public MessagesController(Container container)
        {
            this.container = container;
        }

        #endregion

        [ResponseType(typeof(void))]
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            //Activity activity = (Activity)test;

            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new RootDialog());
            }
            else
            {
                await HandleSystemMessage(activity);
            }

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        private async Task HandleSystemMessage(Activity message)
        {
            try
            {
                switch (message.Type)
                {
                    //case ActivityTypes.ConversationUpdate:
                    //    //ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                    //    //IConversationUpdateActivity update = message;
                    //    //var url_chk = message.ServiceUrl;

                    //    //using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, message))
                    //    //{
                    //    //    var client = scope.Resolve<IConnectorClient>();

                    //    //    if (update.MembersAdded.Any())
                    //    //    {
                    //    //        var reply1 = message.CreateReply();

                    //    //        foreach (var newMember in update.MembersAdded)
                    //    //        {
                    //    //            if (newMember.Id != message.Recipient.Id)
                    //    //            {

                    //    //            }
                    //    //            else
                    //    //            {
                    //    //                string first_message = "Empezar";
                    //    //                reply1.Text = first_message;
                    //    //                await connector.Conversations.ReplyToActivityAsync(reply1);
                    //    //                await Conversation.SendAsync(message, () => new RootDialog());
                    //    //            }
                    //    //        }
                    //    //    }
                    //    //}
                    //    break;

                    case ActivityTypes.ConversationUpdate:
                        // Handle conversation state changes, like members being added and removed
                        // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                        // Not available in all channels
                        //if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                        //{

                        //    //if (message.MembersRemoved != null && message.MembersRemoved?.Count > 1)
                        //    //    await Conversation.SendAsync(message, () => new CerrarSesionDialog());

                           
                        //    var bot = message.MembersAdded.FirstOrDefault();

                        //    //if (bot != null && bot.Name.Equals("Bot")) //TODO: upecito
                        //    if (bot != null)
                        //    {
                        //        //message.CreateReply("Iniciar");
                        //        await Conversation.SendAsync(message, () => new RootDialog());

                        //    }
                        //    else
                        //    {
                        //        if (message.MembersRemoved != null && message.MembersRemoved?.Count > 1)
                        //            await Conversation.SendAsync(message, () => new CerrarSesionDialog());
                        //    }

                        //}
                        //else
                        //{
                        //    bot = message.MembersAdded.FirstOrDefault();

                        //    if (bot != null)
                        //    {
                        //        //message.From.Id = bot.Id;
                        //        //message.From.Name = bot.Name;

                        //        await Conversation.SendAsync(message, () => new RootDialog());
                        //    }

                        //}

                        break;

                    case ActivityTypes.Event:
                        //await Conversation.SendAsync(message, () => new RootDialog());

                        //bot = message.MembersAdded.FirstOrDefault();

                        //if (bot != null)
                        //{
                        //    //message.From.Id = bot.Id;
                        //    //message.From.Name = bot.Name;

                        //    await Conversation.SendAsync(message, () => new WelcomeDialog());
                        //}


                        //if (message.MembersRemoved != null && message.MembersRemoved?.Count > 1)
                        //    await Conversation.SendAsync(message, () => new CerrarSesionDialog());

                        if (message.Name == "requestRootDialog")
                        {
                            //await Conversation.SendAsync(message, () => new RootDialog());

                            ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                            var userId = message.From.Id;

                            var container = new Container();
                            UnityConfig.RegisterTypes(container);

                            var sesion = container.GetInstance<ISesion>();
                            var sesionData = sesion.CrearSesion(ConvertidorUtil.GetLong(userId));

                            //message.MembersAdded = new ChannelAccount[] { new ChannelAccount(message.From.Id, message.From.Name) };
                            
                            //var reply = message.CreateReply("Iniciar");
                            //connector.Conversations.ReplyToActivity(reply);

                            //Activity newActivity = new Activity()
                            //{
                            //    Text = "Iniciar",
                            //    Type = ActivityTypes.Message
                            //    //Conversation = new ConversationAccount
                            //    //{
                            //    //    Id = response.Id
                            //    //},
                            //};

                            //ConversationParameters _params = new ConversationParameters()
                            //{
                            //    Activity = newActivity
                            //};

                            //var response = connector.Conversations.CreateConversation(_params);

                            //newActivity.Conversation.Id = response.Id;

                            //await connector.Conversations.SendToConversationAsync(newActivity);

                            //var hero = message.CreateReply();
                            //var attachment = RootDialog.GetInfoCard();
                            //hero.Attachments.Add(attachment);
                            //await connector.Conversations.ReplyToActivityAsync(hero);

                            //reply = message.CreateReply();
                            //reply.Text = $"Hola {sesionData.Nombre}, soy UPECITO el asesor del Aula Virtual de UPC. Te puedo ayudar con tus consultas académicas y Técnicas del Aula Virtual.";
                            //await connector.Conversations.ReplyToActivityAsync(reply);

                            //Response inputLog = Helpers.PersistChatLog(null, sesionData, message.Text, "Bot", "BotFramework");
                        }

                        await Conversation.SendAsync(message, () => new RootDialog());

                        break;

                    case ActivityTypes.DeleteUserData:
                        // Implement user deletion here
                        // If we handle user deletion, return a real message
                        break;

                    case ActivityTypes.ContactRelationUpdate:
                        // Handle add/remove from contact lists
                        // Activity.From + Activity.Action represent what happened
                        break;

                    case ActivityTypes.Typing:
                        // Handle knowing tha the user is typing
                        break;

                    case ActivityTypes.Ping:
                        break;


                    default:
                        Trace.TraceError($"Unknown activity type ignored: {message.GetActivityType()}");
                        Console.WriteLine(message.Type);
                        break;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
    }
}