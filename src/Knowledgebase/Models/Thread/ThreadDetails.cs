using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.Thread
{
    public class ThreadDetails : IdBaseDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Title { get; set; }
        public ThreadContentDetails Contents { get; set; }
        public Category.CategoryBrief Category { get; set; }
        public ICollection<ThreadContentBrief> Versions { get; set; }
        public ICollection<Tag.TagBrief> Tags { get; set; }
        public string HierarchyString { get; set; }
        public AppUser.AppUserBrief CreatedByUser { get; set; }
        public ICollection<IdNameDto> ParentCategories { get; set; }
        public ICollection<IdNameDto> SiblingThreads { get; set; }
    }
}
