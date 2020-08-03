using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Knowledgebase.Models.Thread
{
    public class ThreadUpdateContents : IdBaseDto
    {
        [Required]
        [MaxLength(4096)]
        public string Contents { get; set; }
    }
}
