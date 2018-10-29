﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleInjector;
using Upecito.Data.Interface;

namespace FormBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private static bool showed = false;

        public async Task StartAsync(IDialogContext context)
        {
            var message = context.MakeMessage();
            var attachment = GetInfoCard();

            message.Attachments.Add(attachment);
            await context.PostAsync(message);

            context.Wait(ShowStartButton);
        }

        public enum StartOptions
        {
            Iniciar
        }

        public virtual async Task ShowStartButton(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: (IEnumerable<StartOptions>)Enum.GetValues(typeof(StartOptions)),
                prompt: "Presiona el botón para iniciar",
                retry: "Por favor intenta de nuevo"
            );
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

        private static Attachment GetInfoCard()
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