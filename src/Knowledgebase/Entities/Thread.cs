using System;
using System.Collections.Generic;

namespace Knowledgebase.Entities
{
    public class Thread : _SoftDeletableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid? UpdatedByUserId { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; }

        public virtual AppUser CreatedByUser { get; set; }
        public virtual AppUser UpdatedByUser { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ThreadContent> Contents { get; set; }
        public virtual ICollection<ThreadTag> Tags { get; set; }
    }
}
