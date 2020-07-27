using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Knowledgebase.Models.Administration;

namespace Knowledgebase.Application.Services
{
    public class AdministrationService : _ServiceBase
    {
        private readonly UtilityServices.AuthService _authService;
        public AdministrationService(IServiceProvider serviceProvider,
            UtilityServices.AuthService authService)
            : base(serviceProvider)
        {
            _authService = authService;
        }

        public async Task Setup(SetupRequest input)
        {
            // check user and permissions
            Session.EnsureAuthenticated();

            // validation

            // save settings
            var appSettingService = this.GetService<AppSettingService>();
            var settings = new Dictionary<string, string>
            {
                { AppSettingService.KEY_Locale, input.Locale },
                { AppSettingService.KEY_CompanyName, input.CompanyName }
            };
            appSettingService.BatchSet(settings);

            //// assign owner role to current user
            //var _authService = GetService<UtilityServices.AuthService>();
            //await _authService.AssignRolesToUser(input.AdminExternalUserId,
            //    new string[] { "deploca:knowledgebase:owner" });

            // create the user
            var externalUser = await _authService.GetUserInfo(input.AdminExternalUserId);
            var appUserId = GetService<AppUserService>()
                .Create(new Models.AppUser.AppUserCreate
                {
                    ExternalId = externalUser.UserId,
                    Picture = externalUser.Picture,
                    Name = externalUser.FullName,
                    Email = externalUser.Email,
                    IsOwner = true
                });
            Session.SetAuthenticatedUserId(appUserId);

            // seed data
            addSeedData(input.Locale);
        }

        private void addSeedData(string locale)
        {
            var category1_title = "Knowledgebase usage guide";
            var thread1_title = "Register users in the system";
            var thread1_contents = "From the top menu, click on login button and click on the 'i dont have an accont' button and enter your information to register.";
            var thread1_tags = new string[] { "knowledgebase" };

            if (locale == "fa")
            {
                category1_title = "راهنمای استفاده از پایگاه دانش";
                thread1_title = "نحوه ثبت نام در سیستم";
                thread1_contents = "در قسمت منوی بالا روی دکمه ورود به سیستم کلیک کرده و ثبت نام را انتخاب نمایید.";
                thread1_tags = new string[] { "پایگاه دانش" };
            }

            var categoryService = this.GetService<CategoryService>();
            var threadService = this.GetService<ThreadService>();
            var category_id = categoryService.Create(new Models.Category.CategoryCreate
            {
                Title = category1_title
            });
            var thread1_id = threadService.Create(new Models.Thread.ThreadCreate
            {
                CategoryId = category_id,
                Title = thread1_title,
                Contents = thread1_contents,
                Tags = thread1_tags.Select(x => new Models.Tag.TagCreateOrUpdate { Name = x }).ToArray()
            });
        }
    }
}
