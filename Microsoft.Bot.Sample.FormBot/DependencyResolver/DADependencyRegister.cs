using Api.Ai.ApplicationService.Factories;
using Api.Ai.Domain.Service.Factories;
using Api.Ai.Infrastructure.Factories;
using FormBot.Dialogflow;
using System;
using Upecito.Business;
using Upecito.Data.Interface;
using Upecito.Interface;

namespace FormBot.DependencyResolver
{
    public class DADependencyRegister
    {
        public static void RegisterTypes(SimpleInjector.Container container, string database)
        {
            container.Register<DialogEngine>();
            container.RegisterInstance<IServiceProvider>(container);

            container.RegisterSingleton<IApiAiAppServiceFactory, ApiAiAppServiceFactory>();
            container.RegisterSingleton<IHttpClientFactory, HttpClientFactory>();
            container.RegisterSingleton<ISolicitud, SolicitudManager>();
            container.RegisterSingleton<IIntencion, IntencionManager>();
            container.RegisterSingleton<ISesion, SesionManager>();

            container.RegisterSingleton<IChatLog, ChatLogManager>();
            container.RegisterSingleton<ICurso, CursoManager>();
            container.RegisterSingleton<IActividad, ActividadManager>();

            if (database.Equals("ORACLE"))
            {
                container.Register<ISesionData, Upecito.Data.Oracle.Implementacion.SesionData>();
                container.Register<ISolicitudData, Upecito.Data.Oracle.Implementacion.SolicitudData>();
                container.Register<IIntencionData, Upecito.Data.Oracle.Implementacion.IntencionData>();
            }

            if (database.Equals("MSSQLSERVER"))
            {
                container.Register<ISesionData, Upecito.Data.MSSQLSERVER.Implementacion.SesionData>();
                container.Register<ISolicitudData, Upecito.Data.MSSQLSERVER.Implementacion.SolicitudData>();
                container.Register<IIntencionData, Upecito.Data.MSSQLSERVER.Implementacion.IntencionData>();
                container.Register<IChatLogData, Upecito.Data.MSSQLSERVER.Implementacion.ChatLogData>();
                container.Register<ICursoData, Upecito.Data.MSSQLSERVER.Implementacion.CursoData>();
                container.Register<IActividadData, Upecito.Data.MSSQLSERVER.Implementacion.ActividadData>();
            }
        }
    }
}