using FormBot.DependencyResolver;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upecito.Interface;
using Upecito.Model;
using Upecito.Model.ViewModel;

namespace FormBot.Dialogs
{
   // [Serializable]
    //public class ProgramacionActividadesDialog : IDialog<object>
   // {
        //public Task StartAsync(IDialogContext context)
        //{
        //    var curso = context.UserData.GetValue<string>("Curso");

        //    if (string.IsNullOrEmpty(curso))
        //    {
        //        var container = new Container();
        //        UnityConfig.RegisterTypes(container);

        //        var cursoManager = container.GetInstance<ICurso>();

        //        Solicitud solicitud = context.UserData.GetValue<Solicitud>("solicitud");

        //        List<CourseByModuleViewModel> studentActiveCourses = cursoManager.GetCourseByModuleActive(solicitud.IdAlumno);

        //        var options = studentActiveCourses.Select(x => x.Curso).ToArray();

        //        PromptDialog.Choice(
        //           context: context,
        //           resume: OnCourseSelected,
        //           options: options,
        //           descriptions: options,
        //           prompt: "Por favor seleccione el curso"
        //       );
        //    }

        //    return Task.CompletedTask;
        //}



    //}
}