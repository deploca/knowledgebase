using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.AppUser
{
    public class AppUserDetails : IdBaseDto
    {
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsOwner { get; set; }
        public string Picture { get; set; }
    }
}
