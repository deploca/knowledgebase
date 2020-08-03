using System;
using System.Collections.Generic;

namespace Knowledgebase.Exceptions
{
    public class ForbiddenRequestException : AppException
    {
        public ForbiddenRequestException(string message = null)
            : base(message)
        {
            Source = this.GetType().Name;
        }
    }
}
