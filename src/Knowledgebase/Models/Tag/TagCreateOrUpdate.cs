using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.Tag
{
    public class TagCreateOrUpdate
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}
