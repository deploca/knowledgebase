using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Knowledgebase.Models.Administration
{
    public class SetupRequest
    {
        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string AdminEmail { get; set; }

        [Required]
        public string AdminPassword { get; set; }
    }
}
