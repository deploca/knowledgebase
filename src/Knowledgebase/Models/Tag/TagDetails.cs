using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.Tag
{
    public class TagDetails : IdBaseDto
    {
        public string Name { get; set; }
        public int ThreadsCount { get; set; }
    }
}
