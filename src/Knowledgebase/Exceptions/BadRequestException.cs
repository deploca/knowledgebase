using System;
using System.Collections.Generic;

namespace Knowledgebase.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string message = null)
            : base(message)
        {
            Source = this.GetType().Name;
        }
    }
}
