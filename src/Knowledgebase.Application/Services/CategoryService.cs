using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Knowledgebase.UnitOfWork;
using Knowledgebase.Models.Category;

namespace Knowledgebase.Application.Services
{
    public class CategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Entities.Category> _categoryRepository;
        public CategoryService(IUnitOfWork uow)
        {
            _uow = uow;
            _categoryRepository = uow.GetRepository<Entities.Category>();
        }

        public ICollection<CategoryBrief> GetAll()
        {
            return _categoryRepository.GetAll()
                .Select(x => new CategoryBrief
                {
                    Id = x.Id,
                    ParentCategoryId = x.ParentCategoryId,
                    Title = x.Title,
                    SubCategoriesCount = x.SubCategories.Count,
                    ThreadsCount = x.Threads.Count,
                })
                .ToList();
        }

        public async Task<Guid> Create(CategoryCreate input)
        {
            var model = new Entities.Category
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ParentCategoryId = input.ParentCategoryId,
                Title = input.Title
            };
            _categoryRepository.Insert(model);
            await _uow.SaveChangesAsync();
            return model.Id;
        }
    }
}
