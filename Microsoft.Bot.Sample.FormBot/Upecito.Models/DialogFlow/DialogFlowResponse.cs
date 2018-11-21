using System;
using System.Collections.Generic;
using System.Text;

namespace Upecito.Models.DialogFlow
{

    public class DialogFlowResponse
    {
        public string ResponseId { get; set; }
        public Queryresult QueryResult { get; set; }
        public Originaldetectintentrequest OriginalDetectIntentRequest { get; set; }
        public string Session { get; set; }
    }

    public class Queryresult
    {
        public Queryresult()
        {
            FulfillmentMessages = new List<FulfillmentMessage>();
        }

        public string QueryText { get; set; }
        public object Parameters { get; set; }
        public bool AllRequiredParamsPresent { get; set; }
        public string FulfillmentText { get; set; }
        public List<FulfillmentMessage> FulfillmentMessages { get; set; }
        public List<OutputContext> OutputContexts { get; set; }

        public Intent Intent { get; set; }
        public float IntentDetectionConfidence { get; set; }
        public string LanguageCode { get; set; }
    }


    public class Intent
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }

    public class FulfillmentMessage
    {
        public FulfillmentText Text { get; set; }
    }

    public class FulfillmentText
    {
        public FulfillmentText()
        {
            Text = new List<string>();
        }

        public List<string> Text { get; set; }
    }

    public class Originaldetectintentrequest
    {
        public object Payload { get; set; }
    }


    public class OutputContext
    {
        public string Name { get; set; }
        public int LifespanCount { get; set; }
        public object Parameters { get; set; }
    }

}
