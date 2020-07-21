using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.Thread
{
    public class ThreadUpdateTags : IdBaseDto
    {
        public Tag.TagCreateOrUpdate[] Tags { get; set; }
    }
}
