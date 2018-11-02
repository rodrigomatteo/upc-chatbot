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
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK);

            //var reply = activity.CreateReply("message.Type: " + activity.Type);
            //ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            //await connector.Conversations.ReplyToActivityAsync(reply);

            if (activity.Type == ActivityTypes.Message || activity.Type == ActivityTypes.ConversationUpdate)
            {
                // Because ConversationUpdate activity is received twice 
                // (once for the Bot being added to the conversation, and the 2nd time - 
                // for the user), we have to filter one out, if we don't want the dialog 
                // to get started twice. Otherwise the user will receive a duplicate message.
                //if (activity.Type == ActivityTypes.ConversationUpdate &&
                //   !activity.MembersAdded.Any(r => r.Name == "Bot"))
                //    return response;

                // start your root dialog here
                await Microsoft.Bot.Builder.Dialogs.Conversation.SendAsync(activity, () =>  new WelcomeDialog());
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
                //  
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