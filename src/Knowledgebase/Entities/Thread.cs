using System;
using System.Collections.Generic;

namespace Knowledgebase.Entities
{
    public class Thread : _SoftDeletableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }

        public virtual Category Category { get; set; }
    }
}
