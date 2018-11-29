using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Upecito.Data.Implementation;

namespace FormBot.Util
{
    /// <summary>
    /// This IPostToBot service converts any unhandled exceptions to a message sent to the user.
    /// </summary>
    public sealed class PostUnhandledExceptionToUser : IPostToBot
    {
        private readonly IPostToBot inner;
        private readonly IBotToUser botToUser;
        private readonly ResourceManager resources;
        private readonly TraceListener trace;

        public PostUnhandledExceptionToUser(IPostToBot inner, IBotToUser botToUser, ResourceManager resources, TraceListener trace)
        {
            SetField.NotNull(out this.inner, nameof(inner), inner);
            SetField.NotNull(out this.botToUser, nameof(botToUser), botToUser);
            SetField.NotNull(out this.resources, nameof(resources), resources);
            SetField.NotNull(out this.trace, nameof(trace), trace);
        }

        async Task IPostToBot.PostAsync(IActivity activity, CancellationToken token)
        {
            try
            {
                await this.inner.PostAsync(activity, token);
            }
            catch(SqlException)
            {
                await this.botToUser.PostAsync("Ocurrió un problema de conexión a la base de datos. Por favor intente más tarde");
            }
            catch (Exception ex)
            {
                try
                {
                    if (ex.Source == "Google.Api.Gax")
                        await this.botToUser.PostAsync("Ocurrió un problema de comunicación con el servidor. Por favor intente más tarde");
                    else
                        //await this.botToUser.PostAsync(this.resources.GetString("UnhandledExceptionToUser"));
                        await this.botToUser.PostAsync("Hay un error");
                }
                catch (Exception inner)
                {
                    trace.WriteLine(inner);
                    BaseData.LogError(inner);
                    await this.botToUser.PostAsync("Hay un error");
                }

                Console.WriteLine(ex.Message);
                BaseData.LogError(ex);
            }
        }
    }
}