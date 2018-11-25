using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Upecito.Model.Common.Enums;

namespace Upecito.Model.Common
{
    public class Response
    {
        public Response()
        {
            Status = false;
            Type = ResponseType.error;
        }

        public bool Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public string Trace { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseType Type { get; set; }
    }
}
