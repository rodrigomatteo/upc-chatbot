﻿using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;
using Upecito.Model;

namespace FormBot.Dialogs
{
    [Serializable]
    public class PreguntasFrecuentesDialog : BaseDialog
    {
        public override async Task StartAsync(IDialogContext context)
        {
            var activity = context.Activity as Activity;

            var result = ObtenerRespuesta(context);

            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            var speech = result.Speech;

            if (speech.Equals(string.Empty))
                speech = @"Por favor canalice su consulta via Contacto UPC: http://www.upc.edu.pe/servicio/contacto-upc";

            var reply = activity.CreateReply(speech);
            await connector.Conversations.ReplyToActivityAsync(reply);

            context.Done(true);
        }

        protected override Result ObtenerRespuesta(IDialogContext context)
        {
            /* El Sistema se conecta con el “Sistema Open DB” solicita la
            programación de actividades para ello envía el nombre de la actividad y
            curso. */
            Result result;
            context.UserData.TryGetValue<Result>("result", out result);
            return result;
        }
    }
}