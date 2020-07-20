using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.Thread
{
    public class ThreadSearch
    {
        public Guid? CategoryId { get; set; }
        public Guid? TagId { get; set; }
        public string Keyword { get; set; }
    }
}
