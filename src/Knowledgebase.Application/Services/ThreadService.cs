using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Knowledgebase.UnitOfWork;
using Knowledgebase.Models.Thread;
using Knowledgebase.Models.Tag;

namespace Knowledgebase.Application.Services
{
    public class ThreadService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Entities.Thread> _threadRepository;
        private readonly IRepository<Entities.Tag> _tagRepository;
        public ThreadService(IUnitOfWork uow)
        {
            _uow = uow;
            _threadRepository = uow.GetRepository<Entities.Thread>();
            _tagRepository = uow.GetRepository<Entities.Tag>();
        }

        public ICollection<ThreadBrief> GetAll(ThreadSearch input)
        {
            var q = _threadRepository.GetAll();
            if (input != null)
            {
                if (input.CategoryId.HasValue)
                    q = q.Where(x => x.CategoryId == input.CategoryId.Value);
                if (input.TagId.HasValue)
                    q = q.Where(x => x.Tags.Select(y => y.TagId).Contains(input.TagId.Value));
                if (!string.IsNullOrWhiteSpace(input.Keyword))
                    q = q.Where(x => x.Title.Contains(input.Keyword));
            }

            return q
                .Select(x => new ThreadBrief
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    Title = x.Title,
                    Tags = x.Tags.Select(y => new Models.Tag.TagBrief
                    {
                        Id = y.TagId,
                        Name = y.Tag.Name
                    }).ToList(),
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
                    Tags = x.Tags.Select(y => new Models.Tag.TagBrief
                    {
                        Id = y.TagId,
                        Name = y.Tag.Name
                    }).ToList(),
                })
                .FirstOrDefault();
        }

        public async Task<Guid> Create(ThreadCreate input)
        {
            // tags
            var tags = new List<Entities.ThreadTag>();
            if (input.Tags != null && input.Tags.Length > 0)
            {
                // existing tags
                //var existingTags = _tagRepository.GetAll()
                //    .Where(x => input.Tags.Contains(x.Name))
                //    .Select(x => new { x.Id, x.Name }).ToArray();
                var existingTags = input.Tags.Where(x => x.Id.HasValue).ToArray();
                tags.AddRange(existingTags.Select(x => new Entities.ThreadTag
                {
                    Id = Guid.NewGuid(),
                    TagId = x.Id.Value,
                }));

                // create new tags
                var newTags = input.Tags.Where(x => x.Id.HasValue == false).ToArray();
                var newTagEntities = newTags
                    .Select(x => new Entities.Tag { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = x.Name })
                    .ToArray();
                _tagRepository.BatchInsert(newTagEntities);
                tags.AddRange(newTagEntities.Select(x => new Entities.ThreadTag
                {
                    Id = Guid.NewGuid(),
                    TagId = x.Id,
                }));
            }

            // create thread
            var model = new Entities.Thread
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                CategoryId = input.CategoryId,
                Title = input.Title,
                Contents = input.Contents,
                Tags = tags,
            };
            _threadRepository.Insert(model);

            // final
            await _uow.SaveChangesAsync();
            return model.Id;
        }


        public ICollection<TagDetails> GetTags()
        {
            return _tagRepository.GetAll()
                .Select(x => new TagDetails
                {
                    Id = x.Id,
                    Name = x.Name,
                    ThreadsCount = x.Threads.Count
                }).ToList();
        }
    }
}
