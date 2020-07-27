using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.Thread
{
    public class ThreadContentBrief : IdBaseDto
    {
        public DateTime CreatedAt { get; set; }
        public AppUser.AppUserBrief CreatedByUser { get; set; }
    }
}
