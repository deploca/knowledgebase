using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Knowledgebase.Models
{
    public class IdBaseDto
    {
        [Required]
        public Guid Id { get; set; }


        public IdBaseDto() { }
        public IdBaseDto(Guid id)
        {
            Id = id;
        }
    }
}
