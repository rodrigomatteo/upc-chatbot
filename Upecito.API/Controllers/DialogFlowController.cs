using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Upecito.Models.Common;
using Upecito.Models.DialogFlow;

namespace Upecito.API.Controllers
{
    public class DialogFlowController : Controller
    {
        [HttpPost]
        public IActionResult Webhook([FromBody] object request)
        {
            Response output = new Response();

            try
            {
                string json = JsonConvert.SerializeObject(request);

                DialogFlowResponse model = JsonConvert.DeserializeObject<DialogFlowResponse>(json);

                output.Message = json;
                output.Status = true;
                output.Type = Enums.ResponseType.success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                output.Message = ex.Message;
                output.Trace = ex.StackTrace;
                output.Data = ex.InnerException;
            }

            return Json(output);
        }
    }
}