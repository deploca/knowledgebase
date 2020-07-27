using System;
using System.Collections.Generic;

namespace Knowledgebase.Exceptions
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException()
        {
            Source = this.GetType().Name;
        }
    }
}
