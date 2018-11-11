using Microsoft.Bot.Builder.Dialogs;
using Upecito.Model;

namespace FormBot.Dialogs
{
    public class CreditosDialog : BaseDialog
    {
        private const string RESPUESTA = "Esta es una pregunta relacionada a Creditos";

        protected override void MostrarRespuesta(IDialogContext context, Result resultado)
        {
            var userName = context.UserData.GetValue<Sesion>("sesion").NombreApePaterno;
            context.PrivateConversationData.SetValue("custom", RESPUESTA);

            resultado.Speech = $"{userName} {RESPUESTA}";
            base.MostrarRespuesta(context, resultado);
        }
    }
}