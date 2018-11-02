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
using System;

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

        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        //[ResponseType(typeof(void))]
        //public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        //{
        //    // check if activity is of type message
        //    if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
        //        await Conversation.SendAsync(activity, () => new WelcomeDialog());
        //    else
        //        HandleSystemMessage(activity);

        //    return new HttpResponseMessage(HttpStatusCode.Accepted);
        //}

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            // DEMO PURPOSE: echo all incoming activities
            Activity reply = activity.CreateReply(Newtonsoft.Json.JsonConvert.SerializeObject(activity, Newtonsoft.Json.Formatting.None));

            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            connector.Conversations.SendToConversation(reply);

            // Process each activity
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new WelcomeDialog());
            }
            // Webchat: getting an "event" activity for our js code
            else if (activity.Type == ActivityTypes.Event && activity.ChannelId == "webchat")
            {
                var receivedEvent = activity.AsEventActivity();

                if ("requestWelcomeDialog".Equals(receivedEvent.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    // DO YOUR GREETINGS FROM HERE
                    await Conversation.SendAsync(activity, () => new WelcomeDialog());
                }
            }
            // Sample for Skype: in ContactRelationUpdate event
            else if (activity.Type == ActivityTypes.ContactRelationUpdate && activity.ChannelId == "skype")
            {
                // DO YOUR GREETINGS FROM HERE
            }
            // Sample for emulator, to debug locales
            else if (activity.Type == ActivityTypes.ConversationUpdate && activity.ChannelId == "emulator")
            {
                foreach (var userAdded in activity.MembersAdded)
                {
                    if (userAdded.Id == activity.From.Id)
                    {
                        // DO YOUR GREETINGS FROM HERE
                    }
                }
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        //public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        //{
        //    if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
        //    {
        //        await Conversation.SendAsync(activity, () => new WelcomeDialog());
        //    }
        //    else
        //    {
        //        HandleSystemMessage(activity);
        //    }
        //    var response = Request.CreateResponse(HttpStatusCode.OK);
        //    return response;
        //}

        private async void HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels

                //IConversationUpdateActivity update = message;
                //var client = new ConnectorClient(new Uri(message.ServiceUrl), new MicrosoftAppCredentials());
                //if (update.MembersAdded != null && update.MembersAdded.Any())
                //{
                //    foreach (var newMember in update.MembersAdded)
                //    {
                //        if (newMember.Id != message.Recipient.Id)
                //        {
                //            await Conversation.SendAsync(message, () => new WelcomeDialog());

                //            var dialog = container.GetInstance<WelcomeDialog>();
                //            await Conversation.SendAsync(message, () => dialog);

                //            var reply = message.CreateReply();
                //            reply.Text = $"Hola {newMember.Name}, soy UPECITO el asesor del Aula Virtual de UPC.Te puedo ayudar con tus consultas acad�micas y T�cnicas del Aula Virtual.";

                //            client.Conversations.ReplyToActivityAsync(reply);
                //        }
                //    }
                //}

                //if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                //{
                //    var bot = message.MembersAdded.FirstOrDefault();
                //    if (bot != null && bot.Name.Equals("Upecito Bot"))
                //    {
                //        var dialog = container.GetInstance<RootDialog>();
                //        await Conversation.SendAsync(message, () => dialog);
                //    }
                //}

                //if (message.MembersRemoved.Count > 1)
                //    await Conversation.SendAsync(message, () => new CerrarSesionDialog());
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }            
        }
    }
}