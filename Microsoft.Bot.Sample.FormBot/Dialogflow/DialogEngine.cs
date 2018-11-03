using System.Net;
using System.Threading.Tasks;
using Api.Ai.ApplicationService.Factories;
using Api.Ai.Domain.DataTransferObject.Request;
using Api.Ai.Domain.Enum;
using Api.Ai.Domain.Service.Factories;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Dialogflow.V2;
using Grpc.Core;
using Grpc.Auth;
using Microsoft.Bot.Connector;
using Upecito.Model;
using System;

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

        public async Task<Result> GetSpeechAsync(Activity message)
        {
            var result = new Result();

            try
            {
                var fileSavePath = System.Web.HttpContext.Current.Server.MapPath("~/Dialogflow/") + AppConstant.DialogFlow.FilePrivateKeyIdJson;

                if ((System.IO.File.Exists(fileSavePath)))
                {
                    var cred = GoogleCredential.FromFile(fileSavePath);
                    var channel = new Channel(SessionsClient.DefaultEndpoint.Host, SessionsClient.DefaultEndpoint.Port, cred.ToChannelCredentials());
                    var client = SessionsClient.Create(channel);

                    var query = new QueryInput
                    {
                        Text = new TextInput
                        {
                            Text = message.Text,
                            LanguageCode = "es-es"
                        }
                    };

                    var sessionId = Guid.NewGuid().ToString();
                    var projectId = AppConstant.DialogFlow.ProjectId;                    
                    var sessionName = new SessionName(projectId, sessionId);

                    var dialogFlow = client.DetectIntent(sessionName, query);
                    var response = dialogFlow.QueryResult;

                    result.SessionId = message.Id;
                    result.Status = (int)HttpStatusCode.OK;
                    result.Speech = response.FulfillmentText;

                    var entity = new Upecito.Model.Intent()
                    {
                        IntentId = response.Intent.IntentName.IntentId,
                        IntentName = response.Intent.DisplayName,
                        Score = response.IntentDetectionConfidence
                    };
                    result.Intents.Add(entity);
                }
                
                //var queryAppService = _apiAiAppServiceFactory.CreateQueryAppService("https://api.api.ai/v1", "adf581038f9a4d3aa7f96479e4ac497e");

                //var queryRequest = new QueryRequest
                //{
                //    Query = new[] { message.Text },
                //    Lang = Language.Spanish,
                //    SessionId = message.Id
                //};

                //result.SessionId = message.Id;
                //var queryResponse = await queryAppService.GetQueryAsync(queryRequest);

                //if (queryResponse?.Result != null)
                //{
                //    if (queryResponse.Status?.Code == (int)HttpStatusCode.OK)
                //    {
                //        result.Status = (int)HttpStatusCode.OK;
                //        if (queryResponse.Result.Fulfillment != null)
                //        //    result.Speech = "Oooops!";
                //        //else if (queryResponse.Result.Fulfillment != null)
                //        {
                //            result.Speech = queryResponse.Result.Fulfillment.Speech;
                //            var entity = new Upecito.Model.Intent()
                //            {
                //                IntentId = queryResponse.Result.Metadata.IntentId,
                //                IntentName = queryResponse.Result.Metadata.IntentName,
                //                Score = queryResponse.Result.Score
                //            };
                //            result.Intents.Add(entity);
                //        }
                //    }
                //}
            }
            catch(Exception ex) {

            }

            return result;
        }
    }
}