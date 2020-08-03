using System;
using System.Collections.Generic;

namespace Knowledgebase.Entities
{
    public class ThreadContent : _BaseEntity
    {
        public Guid ThreadId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedByUserId { get; set; }
        public string Content { get; set; }

        public virtual AppUser CreatedByUser { get; set; }
        public virtual Thread Thread { get; set; }
    }
}
