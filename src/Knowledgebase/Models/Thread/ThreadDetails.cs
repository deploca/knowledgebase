using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.Thread
{
    public class ThreadDetails : IdBaseDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public Category.CategoryBrief Category { get; set; }
        public ICollection<Tag.TagBrief> Tags { get; set; }
    }
}
