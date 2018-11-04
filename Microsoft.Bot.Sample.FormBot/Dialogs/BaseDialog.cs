using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Upecito.Model;

namespace FormBot.Dialogs
{
    public class BaseDialog : IDialog<object>
    {
        public virtual async Task StartAsync(IDialogContext context)
        {
            var resultado = ObtenerRespuesta(context);
            MostrarRespuesta(context, resultado);
            context.Done(true);
        }

        protected virtual Result ObtenerRespuesta(IDialogContext context)
        {
            /* El Sistema se conecta con el “Sistema Open DB” solicita la
            programación de actividades para ello envía el nombre de la actividad y
            curso. */
            return context.UserData.GetValueOrDefault<Result>("result");
        }

        protected virtual void MostrarRespuesta(IDialogContext context, Result resultado)
        {
            var activity = context.Activity as Activity;
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));         
            var reply = activity.CreateReply(resultado.Speech);
            connector.Conversations.ReplyToActivityAsync(reply);
        }
    }
}