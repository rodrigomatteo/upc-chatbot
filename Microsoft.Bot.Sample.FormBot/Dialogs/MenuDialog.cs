using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;
using Upecito.Bot.Upecito.Helpers;
using Upecito.Data.Implementation;
using Upecito.Model;

namespace FormBot.Dialogs
{
    [Serializable]
    public class MenuDialog : IDialog<object>
    {
        private enum Selection
        {
            Consultas_Academicas, Consultas_Tecnicas
        }

        public Task StartAsync(IDialogContext context)
        {
            var options = new[] { Selection.Consultas_Academicas, Selection.Consultas_Tecnicas };
            var descriptions = new[] { "Consultas Académicas", "Consultas y Problemas Técnicos" };

            PromptDialog.Choice(
               context: context,
               resume: OnOptionSelected,
               options: options,
               descriptions: descriptions,
               prompt: "Selecciona el canal de atención en el que requieres ayuda",
               retry: "Por favor intenta de nuevo"
           );

            return Task.CompletedTask;
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<Selection> result)
        {
            var optionSelected = await result;
            var userName = context.UserData.GetValue<Sesion>("sesion").NombreApePaterno;

            switch (optionSelected)
            {
                case Selection.Consultas_Academicas:
                    context.UserData.SetValue("tipo-consulta", AppConstant.CanalAtencion.ConsultasAcademicas);                    
                    PromptDialog.Text(context, ResumeGetAcademicIntent, $"Por favor {userName}, dime tu consulta sobre Consultas Académicas", "Intenta de nuevo");
                    break;
                case Selection.Consultas_Tecnicas:
                    context.UserData.SetValue("tipo-consulta", AppConstant.CanalAtencion.ConsultasTecnicas);                   
                    PromptDialog.Text(context, ResumeGetAcademicIntent, $"Por favor {userName}, dime tu consulta sobre Consultas Ténicas", "Intenta de nuevo");
                    break;
            }

        }

        //public static async Task ResumeGetAcademicIntent(IDialogContext context, IAwaitable<IMessageActivity> result)
        //{
        //    await Helpers.Process(context);
        //}

        public static async Task ResumeGetAcademicIntent(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                await Helpers.Process(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                BaseData.LogError(ex);
            }
        }


        public static async Task ResumeAfterSuccessAcademicIntent(IDialogContext context, IAwaitable<object> result)
        {
            await Helpers.ActualizarSolicitud(context, AppConstant.EstadoSolicitud.ATENDIDO);
            // 4.1.14  El caso de uso finaliza
            context.Wait(MessageReceivedAsync);
        }

        public static async Task ResumeAfterUnknownAcademicIntent(IDialogContext context, IAwaitable<object> result)
        {
            await Helpers.ActualizarSolicitud(context, AppConstant.EstadoSolicitud.INVALIDO);

            // 4.1.14  El caso de uso finaliza
            await Task.Delay(2000);
            //ShowPrompt(context);
        }

        public static async Task ResumeAfterFailedAcademicIntent(IDialogContext context, IAwaitable<object> result)
        {
            await Helpers.ActualizarSolicitud(context, AppConstant.EstadoSolicitud.FALTAINFORMACION);

            // 4.1.14  El caso de uso finaliza
            //await Task.Delay(2000);
            //ShowPrompt(context);

            context.Done(true);
        }

        public static async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            await ResumeGetAcademicIntent(context, new AwaitableFromItem<string>(""));
        }

    }
}