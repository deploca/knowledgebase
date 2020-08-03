using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Knowledgebase.Models.Category
{
    public class CategoryUpdateTitle : IdBaseDto
    {
        [Required]
        [MaxLength(256)]
        public string Title { get; set; }
    }
}
