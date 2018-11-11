using Microsoft.Bot.Builder.Dialogs;
using Upecito.Model;

namespace FormBot.Dialogs
{
    public class CalendarioDialog : BaseDialog
    {
        private const string RESPUESTA = "Esta es una pregunta relacionada a Calendario de Actividades";

        protected override void MostrarRespuesta(IDialogContext context, Result resultado)
        {
            var userName = context.UserData.GetValue<Sesion>("sesion").NombreApePaterno;
            context.PrivateConversationData.SetValue("custom", RESPUESTA);

            resultado.Speech = $"{userName} {RESPUESTA}";
            base.MostrarRespuesta(context, resultado);
        }
    }
}