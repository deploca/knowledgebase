using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.AppUser
{
    public class AppUserBrief : IdBaseDto
    {
        public string Name { get; set; }
        public string Picture { get; set; }
    }
}
