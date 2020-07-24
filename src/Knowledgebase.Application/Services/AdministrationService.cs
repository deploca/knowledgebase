using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Knowledgebase.UnitOfWork;
using Knowledgebase.Models.Administration;
using System.Linq;

namespace Knowledgebase.Application.Services
{
    public class AdministrationService : _ServiceBase
    {
        private readonly IUnitOfWork _uow;
        public AdministrationService(IServiceProvider serviceProvider, IUnitOfWork uow)
            : base(serviceProvider)
        {
            _uow = uow;
        }

        public void Setup(SetupRequest input)
        {
            // validation

            // save settings
            var appSettingService = this.GetService<AppSettingService>();
            var settings = new Dictionary<string, string>
            {
                { AppSettingService.KEY_Locale, input.Locale },
                { AppSettingService.KEY_CompanyName, input.CompanyName }
            };
            appSettingService.BatchSet(settings);

            // create admin user
            // ...

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
