using System;
using System.Collections.Generic;

namespace Knowledgebase.Entities
{
    public class _SoftDeletableEntity : _BaseEntity
    {
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedByUserId { get; set; }
        public virtual AppUser DeletedByUser { get; set; }
    }
}
