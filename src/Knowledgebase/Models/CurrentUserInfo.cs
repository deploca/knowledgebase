using System;
using System.Collections.Generic;

namespace Knowledgebase.Models
{
    public class CurrentUserInfo
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string Locale { get; set; }
    }
}
