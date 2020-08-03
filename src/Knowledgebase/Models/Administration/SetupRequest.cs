using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Knowledgebase.Models.Administration
{
    public class SetupRequest
    {
        [Required]
        public string Locale { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public string AdminExternalUserId { get; set; }
    }
}
