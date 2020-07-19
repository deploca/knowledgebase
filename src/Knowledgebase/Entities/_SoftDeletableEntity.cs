using System;
using System.Collections.Generic;

namespace Knowledgebase.Entities
{
    public class _SoftDeletableEntity : _BaseEntity
    {
        public DateTime? DeletedAt { get; set; }
    }
}
