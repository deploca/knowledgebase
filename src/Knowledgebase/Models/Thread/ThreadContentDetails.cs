using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.Thread
{
    public class ThreadContentDetails : ThreadContentBrief
    {
        public Guid ThreadId { get; set; }
        public string Contents { get; set; }
    }
}
