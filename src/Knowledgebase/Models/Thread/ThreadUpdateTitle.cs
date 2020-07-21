using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Knowledgebase.Models.Thread
{
    public class ThreadUpdateTitle : IdBaseDto
    {
        [Required]
        [MaxLength(256)]
        public string Title { get; set; }
    }
}
