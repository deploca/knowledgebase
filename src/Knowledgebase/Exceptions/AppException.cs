using System;
using System.Collections.Generic;

namespace Knowledgebase.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string message = null)
            : base(message) { }
    }
}
