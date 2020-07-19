using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Knowledgebase.UnitOfWork;
using Knowledgebase.Models.Thread;

namespace Knowledgebase.Application.Services
{
    public class ThreadService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Entities.Thread> _threadRepository;
        public ThreadService(IUnitOfWork uow)
        {
            _uow = uow;
            _threadRepository = uow.GetRepository<Entities.Thread>();
        }

        public ICollection<ThreadBrief> GetAll(ThreadSearch input)
        {
            var q = _threadRepository.GetAll();
            if (input != null)
            {
                if (input.CategoryId.HasValue)
                    q = q.Where(x => x.CategoryId == input.CategoryId.Value);
                if (!string.IsNullOrWhiteSpace(input.Keyword))
                    q = q.Where(x => x.Title.Contains(input.Keyword));
            }

            return q
                .Select(x => new ThreadBrief
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    Title = x.Title,
                })
                .ToList();
        }

        public ThreadDetails GetDetails(Guid id)
        {
            return _threadRepository.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new ThreadDetails
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Title = x.Title,
                    Contents = x.Contents,
                    Category = new Models.Category.CategoryBrief
                    {
                        Id = x.CategoryId,
                        Title = x.Category.Title,
                    },
                })
                .FirstOrDefault();
        }

        public async Task<Guid> Create(ThreadCreate input)
        {
            var model = new Entities.Thread
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                CategoryId = input.CategoryId,
                Title = input.Title,
                Contents = input.Contents,
            };
            _threadRepository.Insert(model);
            await _uow.SaveChangesAsync();
            return model.Id;
        }
    }
}
