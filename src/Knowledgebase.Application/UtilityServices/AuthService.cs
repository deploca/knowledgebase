using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;

namespace Knowledgebase.UtilityServices
{
    public class AuthService
    {
        // cached data
        private string _client_access_token = "";
        private IPagedList<Role> _all_roles;

        // ioc
        private readonly AuthOptions _options;
        private readonly IManagementConnection _managementConnection;
        public AuthService(AuthOptions options, IManagementConnection managementConnection)
        {
            _options = options;
            _managementConnection = managementConnection;
        }

        public async Task GetManagementClientAccessToken(bool regenerate_if_exists = false)
        {
            if (!string.IsNullOrWhiteSpace(_client_access_token) && !regenerate_if_exists)
                return;

            if (!string.IsNullOrWhiteSpace(_options.Management_TestingClientAccessToken))
            {
                _client_access_token = _options.Management_TestingClientAccessToken;
            }
            else
            {
                var client = new AuthenticationApiClient(_options.ServerRootUrl);
                var tokenResponse = await client.GetTokenAsync(new ClientCredentialsTokenRequest
                {
                    ClientId = _options.Management_ClientId,
                    ClientSecret = _options.Management_ClientSecret,
                    Audience = _options.Management_Identifier
                });
                _client_access_token = tokenResponse.AccessToken;
            }
        }

        public async Task LoadRoles()
        {
            if (_all_roles != null)
                return;

            var authManClient = new ManagementApiClient(_client_access_token, _options.ServerRootUrl, _managementConnection);
            _all_roles = await authManClient.Roles.GetAllAsync(new GetRolesRequest());
        }

        public async Task<User> GetUserInfo(string userId)
        {
            await this.GetManagementClientAccessToken();

            var authManClient = new ManagementApiClient(_client_access_token, _options.ServerRootUrl, _managementConnection);
            var userInfoResult = await authManClient.Users.GetAsync(userId);
            return userInfoResult;
        }

        public async Task AssignRolesToUser(string userId, string[] roles)
        {
            await this.GetManagementClientAccessToken();
            await this.LoadRoles();

            var authManClient = new ManagementApiClient(_client_access_token, _options.ServerRootUrl, _managementConnection);
            await authManClient.Users.AssignRolesAsync(userId, new AssignRolesRequest
            {
                Roles = _all_roles
                    .Where(x => roles.Contains(x.Name))
                    .Select(r => r.Id).ToArray()
            });
        }
    }
}
