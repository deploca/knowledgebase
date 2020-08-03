using System;
using System.Collections.Generic;

namespace Knowledgebase.Entities
{
    public class AppSetting : _BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
