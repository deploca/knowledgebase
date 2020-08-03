using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.AppUser
{
    public class AppUserCreate
    {
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public bool IsOwner { get; set; }
    }
}
