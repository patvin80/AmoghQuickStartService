using System.Collections;
using System.Collections.Generic;

namespace quickStartService
{
    public class AssemblySessionData
    {
        public string processName { get; set; }
        public List<AssemblyStep> steps { get; set; }

        public Dictionary<string, string> stepStatus { get; set; }
    }

    public class AssemblyStep
    {
        public string id { get; set; }
        public string title { get; set; }

        public AssemblyStep(string id, string title)
        {
            this.id = id;
            this.title = title;
        }
    }
}