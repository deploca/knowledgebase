using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Knowledgebase.UnitOfWork;

namespace Knowledgebase.Application.Services
{
    public class AppSettingService
    {
        #region Key Constants
        public const string KEY_Locale = "Locale";
        public const string KEY_CompanyName = "CompanyName";
        #endregion

        private readonly IUnitOfWork _uow;
        private readonly IRepository<Entities.AppSetting> _appSettingRepository;
        public AppSettingService(IUnitOfWork uow)
        {
            _uow = uow;
            _appSettingRepository = uow.GetRepository<Entities.AppSetting>();
        }

        public IDictionary<string, string> GetAll()
        {
            return _appSettingRepository.GetAll()
                .ToDictionary(k => k.Key, v => v.Value);
        }

        public string GetValue(string key)
        {
            return _appSettingRepository.GetAll()
                .Where(x => x.Key == key)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public async Task Set(string key, string value)
        {
            var model = new Entities.AppSetting
            {
                Id = Guid.NewGuid(),
                Key = key,
                Value = value,
            };
            _appSettingRepository.Insert(model);
            await _uow.SaveChangesAsync();
        }

        public async Task BatchSet(IDictionary<string, string> items)
        {
            var keys = items.Keys.ToArray();

            // update existing items
            var existingItems = _appSettingRepository.GetAll()
                .Where(x => keys.Contains(x.Key)).ToList();
            foreach(var i in existingItems)
            {
                i.Value = items[i.Key];
                _appSettingRepository.Update(i);
            }

            // add new items
            var newItems = keys
                .Where(k => existingItems.Select(y => y.Key).Contains(k) == false)
                .Select(k => new Entities.AppSetting { Id = Guid.NewGuid(), Key = k, Value = items[k] })
                .ToArray();
            _appSettingRepository.BatchInsert(newItems);

            await _uow.SaveChangesAsync();
        }
    }
}
