using Api.Ai.ApplicationService.Factories;
using Api.Ai.Domain.Service.Factories;
using FormBot.Models;
using FormBot.Services;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Auth;
using Grpc.Core;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Upecito.Interface;
using Upecito.Model;
using Intent = Upecito.Model.Intent;

namespace FormBot.Dialogflow
{
    public class DialogEngine
    {
        #region Private Fields

        private readonly IApiAiAppServiceFactory _apiAiAppServiceFactory;
        private readonly IHttpClientFactory _httpClientFactory;

        #endregion

        public DialogEngine(IApiAiAppServiceFactory apiAiAppServiceFactory, IHttpClientFactory httpClientFactory)
        {
            _apiAiAppServiceFactory = apiAiAppServiceFactory;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Result> GetSpeechAsync(Activity message, Sesion sesion, IDialogContext context)
        {
            Result result = new Result();

            try
            {
                string fileSavePath = System.Web.HttpContext.Current.Server.MapPath("~/Dialogflow/") + AppConstant.DialogFlow.FilePrivateKeyIdJson;

                if (System.IO.File.Exists(fileSavePath))
                {
                    GoogleCredential cred = GoogleCredential.FromFile(fileSavePath);
                    Channel channel = new Channel(SessionsClient.DefaultEndpoint.Host, SessionsClient.DefaultEndpoint.Port, cred.ToChannelCredentials());
                    SessionsClient client = SessionsClient.Create(channel);

                    QueryInput query = new QueryInput
                    {
                        Text = new TextInput
                        {
                            Text = message.Text,
                            LanguageCode = "es-es"
                        }
                    };

                    //string sessionId = Guid.NewGuid().ToString();
                    //string projectId = "upc-chatbot"; //TODO: Move to AppSettings

                    string sessionId = sesion.IdSesion.ToString();
                    string projectId = ConfigurationManager.AppSettings["ApiAiProjectId"].ToString();

                    SessionName sessionName = new SessionName(projectId, sessionId);

                    RepeatedField<Context> outputContexts = new RepeatedField<Context>();

                    List<DialogFlowContext> dialogFlowContexts = new List<DialogFlowContext>();

                    if (context.UserData.ContainsKey("OutputContexts"))
                    {
                        string outputContextsGetValue = context.UserData.GetValue<string>("OutputContexts");

                        if (!string.IsNullOrEmpty(outputContextsGetValue))
                        {
                            dialogFlowContexts = JsonConvert.DeserializeObject<List<DialogFlowContext>>(outputContextsGetValue);

                            foreach (var item in dialogFlowContexts)
                            {
                                Struct s = new Struct();

                                foreach (var p in item.Parameters)
                                {
                                    Value v = new Value
                                    {
                                        StructValue = null,
                                        ListValue = null,
                                        NumberValue = 0D,
                                        StringValue = p.Value //This order position matters!!!
                                    };

                                    s.Fields.Add(p.Key, v);
                                }

                                outputContexts.Add(new Context { Name = item.Name, LifespanCount = item.LifespanCount, Parameters = s });
                            }

                        }

                    }

                    DetectIntentRequest detectIntentRequest = new DetectIntentRequest
                    {
                        SessionAsSessionName = sessionName,
                        QueryInput = query,
                        QueryParams = new QueryParameters()
                    };

                    if (outputContexts.Count > 0)
                    {
                        detectIntentRequest.QueryParams.Contexts.AddRange(outputContexts);

                    }

                    DetectIntentResponse dialogFlow = client.DetectIntent(detectIntentRequest);

                    QueryResult response = dialogFlow.QueryResult;

                    string outputContextsSetValue = response.OutputContexts.ToString();

                    context.UserData.SetValue("OutputContexts", outputContextsSetValue);

                    await EvaluateDialogFlowResponse(response, result, message, sesion);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        private async Task EvaluateDialogFlowResponse(QueryResult response, Result result, Activity message, Sesion sesion)
        {
            result.SessionId = message.Id;
            result.Status = (int)HttpStatusCode.OK;
            result.Speech = response.FulfillmentText;

            result.OutputContexts = response.OutputContexts.ToString();

            Intent entity = new Intent()
            {
                IntentId = response.Intent.IntentName.IntentId,
                IntentName = response.Intent.DisplayName,
                Score = response.IntentDetectionConfidence,
                Parameters = response.Parameters.ToString(),
                AllRequiredParamsPresent = response.AllRequiredParamsPresent
            };

            result.Intents.Add(entity);

            //TODO: Create chatlog record
            PersistChatLog(result, response, sesion);

            //TODO: bool IsEmailSent = await SmtpEmailSender.SendEmailAsync("upc.chatbot@gmail.com", "parismiguel@gmail.com", "UPECITO - Email test", JsonConvert.SerializeObject(result));
        }

        private void PersistChatLog(Result result, QueryResult response, Sesion sesion)
        {
            try
            {
                var container = new Container();
                DependencyResolver.UnityConfig.RegisterTypes(container);
                IChatLog chatlog = container.GetInstance<IChatLog>();

                ChatLog input = new ChatLog
                {
                    IdSesion = (int)sesion.IdSesion,
                    IdAlumno = (int)sesion.IdAlumno,
                    Fecha = DateTime.Now,
                    Texto = response.QueryText,
                    Intencion = response.Intent.DisplayName,
                    Fuente = "Usuario",
                    Contexto = response.OutputContexts.ToString(),
                    Parametros = response.Parameters.ToString(),
                    Confianza = (decimal)response.IntentDetectionConfidence

                };

                ChatLog chatLogInputData = chatlog.CrearChatLog(input);

                ChatLog output = new ChatLog
                {
                    IdSesion = (int)sesion.IdSesion,
                    IdAlumno = (int)sesion.IdAlumno,
                    Fecha = DateTime.Now,
                    Texto = response.FulfillmentText,
                    Intencion = response.Intent.DisplayName,
                    Fuente = "Bot",
                    Contexto = response.OutputContexts.ToString(),
                    Parametros = response.Parameters.ToString(),
                    Confianza = (decimal)response.IntentDetectionConfidence

                };

                ChatLog chatLogOutputData = chatlog.CrearChatLog(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}