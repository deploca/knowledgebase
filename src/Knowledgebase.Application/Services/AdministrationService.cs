using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Knowledgebase.UnitOfWork;
using Knowledgebase.Models.Administration;

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

        public async Task Setup(SetupRequest input)
        {
            // validation

            // save settings
            var appSettingService = this.GetService<AppSettingService>();
            var settings = new Dictionary<string, string>
            {
                { AppSettingService.KEY_CompanyName, input.CompanyName }
            };
            await appSettingService.BatchSet(settings);

            // create admin user
            // ...

            // seed data
            var categoryService = this.GetService<CategoryService>();
            var threadService = this.GetService<ThreadService>();
            var category_id = await categoryService.Create(new Models.Category.CategoryCreate
            {
                Title = "راهنمای استفاده از پایگاه دانش"
            });
            var thread1_id = await threadService.Create(new Models.Thread.ThreadCreate
            {
                CategoryId = category_id,
                Title = "نحوه ثبت نام در سیستم",
                Contents = "در قسمت منوی بالا روی دکمه ورود به سیستم کلیک کرده و ثبت نام را انتخاب نمایید.",
            });

            await _uow.SaveChangesAsync();
        }
    }
}
