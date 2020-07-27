using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.Thread
{
    public class ThreadBrief : IdBaseDto
    {
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
        public ICollection<Tag.TagBrief> Tags { get; set; }
        public AppUser.AppUserBrief CreatedByUser { get; set; }
    }
}
