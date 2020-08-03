using System;
using System.Collections.Generic;

namespace Knowledgebase.Models
{
    public class IdNameDto : IdBaseDto
    {
        public string Name { get; set; }


        public IdNameDto() { }
        public IdNameDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
