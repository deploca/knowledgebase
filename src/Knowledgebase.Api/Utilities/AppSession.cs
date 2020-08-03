using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Knowledgebase.Api.Utilities
{
    public class AppSession : UtilityServices.IAppSession
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppSession(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
        }

        public bool IsUserAuthenticated =>
            _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

        private Guid? authenticatedUserId;
        public Guid? AuthenticatedUserId
        {
            get
            {
                if (authenticatedUserId.HasValue)
                    return authenticatedUserId.Value;
                else if (this.IsUserAuthenticated)
                {
                    var externalUserId = _httpContextAccessor.HttpContext
                        .User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var appUserRepository = _serviceProvider
                        .GetRequiredService<UnitOfWork.IUnitOfWork>()
                        .GetRepository<Entities.AppUser>();

                    this.authenticatedUserId = appUserRepository.GetAll()
                        .Where(x => x.ExternalId == externalUserId)
                        .Select(x => x.Id)
                        .FirstOrDefault();

                    if (this.authenticatedUserId == default)
                        this.authenticatedUserId = null;

                    return this.authenticatedUserId;
                }
                return null;
            }
        }

        public void SetAuthenticatedUserId(Guid id)
        {
            this.authenticatedUserId = id;
        }

        public void EnsureAuthenticated()
        {
            if (!IsUserAuthenticated)
                throw new Exceptions.UnauthorizedException();
        }

        //public void EnsurePermissions(params string[] permissions)
        //{
        //    var rawUserPermissions = _httpContextAccessor.HttpContext.User.FindFirstValue("permission");
        //    var listUserPermissions = rawUserPermissions.StartsWith("[") && rawUserPermissions.EndsWith("]") ?
        //        System.Text.Json.JsonSerializer.Deserialize<string[]>(rawUserPermissions) :
        //        new string[] { rawUserPermissions };

        //    var satisfied = permissions.Any(x => listUserPermissions.Contains(x));
        //    if (!satisfied)
        //        throw new Exceptions.UnauthorizedException();
        //}
    }
}
