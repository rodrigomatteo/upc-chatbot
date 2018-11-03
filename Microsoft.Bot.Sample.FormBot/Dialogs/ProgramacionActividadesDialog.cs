using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleInjector;
using System;
using System.Threading.Tasks;
using Upecito.Model;

namespace FormBot.Dialogs
{
    [Serializable]
    public class ProgramacionActividadesDialog : IDialog<object>
    {
        private enum Selection
        {
            Tarea, Foro, Cuestinario, SesionVirtual, Evaluacion
        }

        private const string respuesta = "Esta es una pregunta relacionada a Programación Académica";

        public Task StartAsync(IDialogContext context)
        {
            var userName = context.UserData.GetValue<Sesion>("sesion").NombreApePaterno;                       

            //context.PostAsync($"{userName} {respuesta}");

            context.Wait(ShowPrompt);

            return Task.CompletedTask;
        }
               

        private Task ShowPrompt(IDialogContext context, IAwaitable<object> result)
        {
            var options = new[] { Selection.Tarea, Selection.Foro, Selection.Cuestinario, Selection.SesionVirtual, Selection.Evaluacion };
            var descriptions = new[] { "Tarea", "Foro", "Cuestionario", "Sesion Virtual", "Evaluacion" };

            PromptDialog.Choice<Selection>(context, OnOptionSelected, options, "Selecciona la actividad que deseas consultar", descriptions: descriptions);

            return Task.CompletedTask;
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<Selection> result)
        {
            var optionSelected = await result;
            var userName = context.UserData.GetValue<Sesion>("sesion").NombreApePaterno;

            switch (optionSelected)
            {
                case Selection.Tarea:
                    context.UserData.SetValue("tipo-actividad", AppConstant.ProgramacionAcademica.TipoActividad.Tarea);
                    PromptDialog.Text(context, ResumeGetAcademicIntent, $"Por favor {userName}, dime tu consulta sobre Consultas Académicas", "Intenta de nuevo");
                    break;
                case Selection.Foro:
                    context.UserData.SetValue("tipo-consulta", AppConstant.ProgramacionAcademica.TipoActividad.Foro);
                    PromptDialog.Text(context, ResumeGetAcademicIntent, $"Por favor {userName}, dime tu consulta sobre Consultas Ténicas", "Intenta de nuevo");
                    break;
            }
        }
        
        private async Task ResumeGetAcademicIntent(IDialogContext context, IAwaitable<string> result)
        {
            await Process(context);
        }

        private async Task Process(IDialogContext context)
        {

        }
    }
}