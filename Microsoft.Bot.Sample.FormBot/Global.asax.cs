using Autofac;
using System;
using System.Web.Http;
using System.Configuration;
using System.Reflection;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;

namespace SimpleEchoBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Bot Storage: This is a great spot to register the private state storage for your bot. 
            // We provide adapters for Azure Table, CosmosDb, SQL Azure, or you can implement your own!
            // For samples and documentation, see: https://github.com/Microsoft/BotBuilder-Azure

            Conversation.UpdateContainer(
                builder =>
                {
                    builder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));

                    // Using Azure Table Storage
                    var store = new TableBotDataStore(ConfigurationManager.AppSettings["AzureWebJobsStorage"]); // requires Microsoft.BotBuilder.Azure Nuget package 

                    // To use CosmosDb or InMemory storage instead of the default table storage, uncomment the corresponding line below
                    // var store = new DocumentDbBotDataStore("cosmos db uri", "cosmos db key"); // requires Microsoft.BotBuilder.Azure Nuget package 
                    // var store = new InMemoryDataStore(); // volatile in-memory store

                    builder.Register(c => store)
                        .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                        .AsSelf()
                        .SingleInstance();

                });

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //var exc = Server.GetLastError();
            //var iexc = exc.InnerException;

            //// perform logging
            //System.IO.StreamWriter writer = null;
            //try
            //{
            //    var path = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + System.IO.Path.DirectorySeparatorChar;
            //    var logFileFolder = string.Format("{0}Log{1}{2}{3}", path, System.IO.Path.DirectorySeparatorChar, DateTime.Now.ToString("MMM-yyyy"), System.IO.Path.DirectorySeparatorChar);

            //    if (!System.IO.Directory.Exists(logFileFolder))
            //        System.IO.Directory.CreateDirectory(logFileFolder);

            //    writer = System.IO.File.AppendText($"{logFileFolder}Bot_Error_[{DateTime.Now:ddMMMyyyy}].log");

            //    writer.WriteLine($"DATE AND TIME OF EXCEPTION : {DateTime.Now:MM/dd/yyyy HH:mm:ss}");

            //    writer.WriteLine($"MACHINE NAME               : {Environment.MachineName}");
            //    writer.WriteLine($"SOURCE PATH                : {Request.Path}");
            //    writer.WriteLine($"EXCEPTION SOURCE           : {exc.Source}");
            //    writer.WriteLine($"EXCEPTION TYPE             : {exc.GetType().FullName}");
            //    writer.WriteLine($"EXCEPTION MESSAGE          : {exc.Message}");
            //    writer.WriteLine($"EXCEPTION STACK TRACE      : {exc.StackTrace}");

            //    if (iexc != null)
            //    {
            //        writer.WriteLine($"INNER EXCEPTION SOURCE     : {iexc.Source}");
            //        writer.WriteLine($"INNER EXCEPTION TYPE       : {iexc.GetType().FullName}");
            //        writer.WriteLine($"INNER EXCEPTION MESSAGE    : {iexc.Message}");
            //        writer.WriteLine($"INNER EXCEPTION STACK TRACE: {iexc.StackTrace}");
            //    }

            //    writer.WriteLine($"FULL EXCEPTION             : {exc}");
            //    writer.WriteLine($"CALL STACK                 : {Environment.StackTrace}");

            //    writer.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            //    writer.Flush();
            //}
            //catch (Exception ex)
            //{
            //    writer.WriteLine("Exception: " + ex.Message);
            //}
            //finally
            //{
            //    if (writer != null)
            //        writer.Close();
            //}
        }
    }
}
