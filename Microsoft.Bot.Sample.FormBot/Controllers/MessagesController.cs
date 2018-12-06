using FormBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleInjector;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

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

            //TODO: For testing purposes
            if (activity.ChannelId == "emulator")
            {
                //activity.From.Id = "1"; //U201502689 - idPersonas = 1 - LISSETH
                //activity.From.Id = "7"; //U201203752 - idPersonas = 10 - LIZ
                activity.From.Id = "2"; //U201115024

                if (activity.Type == ActivityTypes.ConversationUpdate)
                {
                    activity.Type = ActivityTypes.Event;
                    activity.Name = "requestRootDialog";

                }
            }

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

                    case ActivityTypes.ConversationUpdate:

                        if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                        {
                            if (message.MembersRemoved != null && message.MembersRemoved?.Count > 1)
                            {
                                await Conversation.SendAsync(message, () => new CerrarSesionDialog());
                            }
                        }

                        break;

                    case ActivityTypes.Event:

                        if (message.Name == "requestRootDialog")
                        {
                            var bot = message.MembersAdded.FirstOrDefault();

                            if (bot != null)
                            {
                                if (bot.Name.Equals("Bot") || bot.Name.Equals("upecito"))
                                {
                                    await Conversation.SendAsync(message, () => new RootDialog());
                                }
                            }
                        }


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