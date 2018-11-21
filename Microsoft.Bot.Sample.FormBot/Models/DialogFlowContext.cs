using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormBot.Models
{
    public class DialogFlowContext
    {
        public string Name { get; set; }

        public int LifespanCount { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

    }
}