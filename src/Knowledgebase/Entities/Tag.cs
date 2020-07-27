using System;
using System.Collections.Generic;

namespace Knowledgebase.Entities
{
    public class Tag : _BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public Guid CreatedByUserId { get; set; }
        public string Name { get; set; }

        public virtual AppUser CreatedByUser { get; set; }
        public virtual ICollection<ThreadTag> Threads { get; set; }
    }
}
