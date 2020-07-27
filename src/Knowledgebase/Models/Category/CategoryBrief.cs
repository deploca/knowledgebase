using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.Category
{
    public class CategoryBrief : IdBaseDto
    {
        public Guid? ParentCategoryId { get; set; }
        public string Title { get; set; }
        public int ThreadsCount { get; set; }
        public int SubCategoriesCount { get; set; }
        public AppUser.AppUserBrief CreatedByUser { get; set; }
    }
}
