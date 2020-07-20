using System;
using System.Collections.Generic;

namespace Knowledgebase.Entities
{
    public class ThreadTag : _BaseEntity
    {
        public Guid ThreadId { get; set; }
        public Guid TagId { get; set; }

        public virtual Thread Thread { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
