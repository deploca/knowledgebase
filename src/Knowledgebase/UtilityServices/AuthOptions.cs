using System;
using System.Collections.Generic;

namespace Knowledgebase.UtilityServices
{
    public class AuthOptions
    {
        public string ServerRootUrl { get; set; }

        public string Management_ClientId { get; set; }
        public string Management_ClientSecret { get; set; }
        public string Management_Identifier { get; set; }
        public string Management_TestingClientAccessToken { get; set; }

        public string UiApp_ClientId { get; set; }
        public string UiApp_ClientSecret { get; set; }
        public string UiApp_Identifier { get; set; }
    }
}
