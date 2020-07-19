using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.Thread
{
    public class ThreadDetails
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public Category.CategoryBrief Category { get; set; }
    }
}
