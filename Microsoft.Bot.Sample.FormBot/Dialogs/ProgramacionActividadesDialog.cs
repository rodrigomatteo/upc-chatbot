using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleInjector;
using System;
using System.Threading.Tasks;
using Upecito.Model;

namespace FormBot.Dialogs
{
    public class ProgramacionActividadesDialog : BaseDialog
    {
        private const string respuesta = "Esta es una pregunta relacionada a Programación Académica";

        protected override void MostrarRespuesta(IDialogContext context, Result resultado)
        {
            var userName = context.UserData.GetValue<Sesion>("sesion").NombreApePaterno;

            resultado.Speech = $"{userName} {respuesta} {resultado.Speech}";
            base.MostrarRespuesta(context, resultado);
        }
    }
}