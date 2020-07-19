using System;
using System.Collections.Generic;

namespace Knowledgebase.Models.Category
{
    public class CategoryBrief
    {
        public Guid Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Title { get; set; }
    }
}
