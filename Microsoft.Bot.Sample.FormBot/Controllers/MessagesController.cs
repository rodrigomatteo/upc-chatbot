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
using FormBot;

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
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK);

            if (activity.Type == ActivityTypes.Message)
            {
                // start your root dialog here
                //await Microsoft.Bot.Builder.Dialogs.Conversation.SendAsync(activity, () =>  new WelcomeDialog());
            }
            // handle other types of activity here (other than Message and ConversationUpdate 
            // activity types)
            else
            {
                HandleSystemMessage(activity);
            }

            return response;
        }

        private async void HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                if (message.MembersAdded != null && message.MembersAdded.Any())
                {
                    foreach (var newMember in message.MembersAdded)
                    {
                        if (newMember.Name == "upc-chatbot")
                        {
                            IMessageActivity greetingMessage = Activity.CreateMessageActivity();

                            //...
                            //your code logic
                            //...

                            IMessageActivity dialogEntryMessage = Activity.CreateMessageActivity();
                            dialogEntryMessage.Recipient = message.Recipient;//to bot
                            dialogEntryMessage.From = message.From;//from user
                            dialogEntryMessage.Conversation = message.Conversation;
                            dialogEntryMessage.Text = "show choices";
                            dialogEntryMessage.Locale = "es-es";
                            dialogEntryMessage.ChannelId = message.ChannelId;
                            dialogEntryMessage.ServiceUrl = message.ServiceUrl;
                            dialogEntryMessage.Id = System.Guid.NewGuid().ToString();
                            dialogEntryMessage.ReplyToId = greetingMessage.Id;

                            await Conversation.SendAsync(dialogEntryMessage, () => new WelcomeDialog());
                        }
                    }
                }
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
            else
            {
               
            }            
        }
    }
}