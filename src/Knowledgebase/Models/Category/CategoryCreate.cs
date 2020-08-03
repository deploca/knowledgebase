using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Knowledgebase.Models.Category
{
    public class CategoryCreate
    {
        public Guid? ParentCategoryId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Title { get; set; }
    }
}
