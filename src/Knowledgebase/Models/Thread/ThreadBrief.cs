using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.Thread
{
    public class ThreadBrief
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
    }
}
