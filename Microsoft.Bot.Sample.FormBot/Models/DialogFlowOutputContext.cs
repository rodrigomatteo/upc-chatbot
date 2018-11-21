using System.Collections.Generic;

namespace FormBot.Models
{
    public class DialogFlowOutputContext
    {
        public DialogFlowOutputContext()
        {
            OutputContexts = new List<OutputContext>();
        }

        public List<OutputContext> OutputContexts { get; set; }
    }

    public class OutputContext
    {
        public string Name { get; set; }
        public int LifespanCount { get; set; }
        public object Parameters { get; set; }
    }

}