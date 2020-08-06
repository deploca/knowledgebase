using System;
using System.Collections.Generic;

namespace Knowledgebase.UtilityServices
{
    public class AuthOptions
    {
        public string ServerRootUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ApiIdentifier { get; set; }
        public string TestingClientAccessToken { get; set; }
    }
}
