using System;
using System.Collections.Generic;

namespace Knowledgebase.UtilityServices
{
    public interface IAppSession
    {
        bool IsUserAuthenticated { get; }
        Guid? AuthenticatedUserId { get; }
        void SetAuthenticatedUserId(Guid id);
        void EnsureAuthenticated();
        //void EnsurePermissions(params string[] permissions);
    }
}
