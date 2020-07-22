using System;
using System.Collections.Generic;

namespace Knowledgebase.Entities
{
    public class Category : _SoftDeletableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Hierarchy { get; set; }
        public string Title { get; set; }

        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; }
        public virtual ICollection<Thread> Threads { get; set; }
    }
}
