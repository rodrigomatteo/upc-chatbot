using Microsoft.Bot.Builder.Dialogs;
using Upecito.Model;

namespace FormBot.Dialogs
{
    public class OrganizacionDialog : BaseDialog
    {
        private const string RESPUESTA = "Esta es una pregunta relacionada a Organización de Aula Virtual";

        protected override void MostrarRespuesta(IDialogContext context, Result resultado)
        {
            var userName = context.UserData.GetValue<Sesion>("sesion").NombreApePaterno;

            resultado.Speech = $"{userName} {RESPUESTA}";
            base.MostrarRespuesta(context, resultado);
        }
    }
}