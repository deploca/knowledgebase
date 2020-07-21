using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Knowledgebase.Models.Thread
{
    public class ThreadUpdate
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Title { get; set; }

        [Required]
        [MaxLength(4096)]
        public string Contents { get; set; }

        public Tag.TagCreateOrUpdate[] Tags { get; set; }
    }
}
