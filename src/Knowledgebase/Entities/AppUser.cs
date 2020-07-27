using System;
using System.Collections.Generic;

namespace Knowledgebase.Entities
{
    public class AppUser : _BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsOwner { get; set; }
        public string Picture { get; set; }
    }
}
