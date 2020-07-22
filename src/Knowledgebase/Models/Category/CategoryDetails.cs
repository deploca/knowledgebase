using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.Category
{
    public class CategoryDetails : IdBaseDto
    {
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string HierarchyString { get; set; }
        public ICollection<IdNameDto> ParentCategories { get; set; }
        public ICollection<IdNameDto> SiblingCategories { get; set; }
        public ICollection<CategoryBrief> SubCategories { get; set; }
        public ICollection<Thread.ThreadBrief> Threads { get; set; }
    }
}
