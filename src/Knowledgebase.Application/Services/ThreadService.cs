using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Knowledgebase.UnitOfWork;
using Knowledgebase.Models;
using Knowledgebase.Models.Tag;
using Knowledgebase.Models.Thread;
using Knowledgebase.Models.Category;

namespace Knowledgebase.Application.Services
{
    public class ThreadService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Entities.Thread> _threadRepository;
        private readonly IRepository<Entities.ThreadContent> _threadContentRepository;
        private readonly IRepository<Entities.Category> _categoryRepository;
        private readonly IRepository<Entities.Tag> _tagRepository;
        public ThreadService(IUnitOfWork uow)
        {
            _uow = uow;
            _threadRepository = uow.GetRepository<Entities.Thread>();
            _threadContentRepository = uow.GetRepository<Entities.ThreadContent>();
            _categoryRepository = uow.GetRepository<Entities.Category>();
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

            // sort
            q = q.OrderBy(x => x.Title);

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
            var data = _threadRepository.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new ThreadDetails
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Title = x.Title,
                    HierarchyString = (x.Category.ParentCategoryId.HasValue ? x.Category.Hierarchy + ";" : "") + x.CategoryId.ToString(),
                    Contents = x.Contents.OrderByDescending(c => c.CreatedAt)
                        .Select(c => new ThreadContentDetails
                        {
                            Id = c.Id,
                            ThreadId = c.ThreadId,
                            CreatedAt = c.CreatedAt,
                            Contents = c.Content,
                        }).FirstOrDefault(),
                    Versions = x.Contents.OrderByDescending(c => c.CreatedAt)
                        .Select(c => new ThreadContentBrief { Id = c.Id, CreatedAt = c.CreatedAt }).ToList(),
                    Category = new CategoryBrief
                    {
                        Id = x.CategoryId,
                        Title = x.Category.Title,
                    },
                    Tags = x.Tags.Select(y => new TagBrief
                    {
                        Id = y.TagId,
                        Name = y.Tag.Name
                    }).OrderBy(y => y.Name).ToList(),
                })
                .FirstOrDefault();

            // calculate parent categories
            if (string.IsNullOrWhiteSpace(data.HierarchyString) == false)
            {
                var hierarchyIds = data.HierarchyString.Split(';').Select(x => Guid.Parse(x)).ToList();
                data.ParentCategories = _categoryRepository.GetAll()
                    .Where(x => hierarchyIds.Contains(x.Id))
                    .Select(x => new IdNameDto(x.Id, x.Title)).ToList()
                    .OrderBy(x => hierarchyIds.IndexOf(x.Id)).ToList();
            }
            else
                data.ParentCategories = new List<IdNameDto>();

            // calculate sibling threads
            data.SiblingThreads = _threadRepository.GetAll()
                .Where(x => x.CategoryId == data.Category.Id)
                .OrderBy(x => x.Title)
                .Select(x => new IdNameDto(x.Id, x.Title)).ToList();

            return data;
        }

        public async Task<Guid> Create(ThreadCreate input)
        {
            // validation
            var sameNameExist = _threadRepository.GetAll()
                .Where(x => x.CategoryId == input.CategoryId && x.Title == input.Title).Any();
            if (sameNameExist)
                throw new Exceptions.BadRequestException("THREAD_WITH_SAME_NAME_EXIST");

            // tags
            var tags = new List<Entities.ThreadTag>();
            if (input.Tags != null && input.Tags.Length > 0)
            {
                // existing tags
                var existingTags = input.Tags.Where(x => x.Id.HasValue);
                tags.AddRange(existingTags.Select(x => new Entities.ThreadTag
                {
                    Id = Guid.NewGuid(),
                    TagId = x.Id.Value,
                }));

                // create new tags
                var newTags = input.Tags.Where(x => x.Id.HasValue == false);
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
                Contents = new List<Entities.ThreadContent>() {
                    new Entities.ThreadContent {
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow,
                        Content = input.Contents
                    }
                },
                Tags = tags,
            };
            _threadRepository.Insert(model);

            // final
            await _uow.SaveChangesAsync();
            return model.Id;
        }

        public async Task UpdateThread(ThreadUpdate input)
        {
            var threadModel = _threadRepository.Find(input.Id);
            var latestContent = _threadContentRepository.GetAll()
                .Where(x => x.ThreadId == input.Id)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();
            var tags = _threadRepository.GetAll().Where(x => x.Id == input.Id)
                .SelectMany(x => x.Tags).ToList();

            // contents
            if (latestContent.Content != input.Contents)
            {
                _threadContentRepository.Insert(new Entities.ThreadContent
                {
                    Id = Guid.NewGuid(),
                    ThreadId = input.Id,
                    CreatedAt = DateTime.UtcNow,
                    Content = input.Contents,
                });
            }

            // tags
            if (input.Tags != null && input.Tags.Length > 0)
            {
                // remove removed tags
                tags.RemoveAll(x => input.Tags.Select(y => y.Id).Contains(x.TagId) == false);

                // add new existing tags
                var newExistingTags = input.Tags.Where(x => x.Id.HasValue && tags.Select(y => y.TagId).Contains(x.Id.Value) == false);
                tags.AddRange(newExistingTags.Select(x => new Entities.ThreadTag
                {
                    Id = Guid.NewGuid(),
                    TagId = x.Id.Value,
                }));

                // add new not-existing tags
                var newNotExistingTags = input.Tags.Where(x => x.Id.HasValue == false);
                var newNotExistingTagEntities = newNotExistingTags
                    .Select(x => new Entities.Tag { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = x.Name })
                    .ToArray();
                _tagRepository.BatchInsert(newNotExistingTagEntities);
                tags.AddRange(newNotExistingTagEntities.Select(x => new Entities.ThreadTag
                {
                    Id = Guid.NewGuid(),
                    TagId = x.Id,
                }));
            }
            else
            {
                tags.RemoveAll(x => true);
            }

            threadModel.Title = input.Title;
            threadModel.UpdatedAt = DateTime.UtcNow;
            threadModel.Tags = tags;
            _threadRepository.Update(threadModel);

            await _uow.SaveChangesAsync();
        }

        public async Task UpdateThreadTitle(ThreadUpdateTitle input)
        {
            var threadModel = _threadRepository.Find(input.Id);

            if (threadModel.Title == input.Title)
                throw new Exceptions.BadRequestException("NOTHING_CHANGED");

            threadModel.Title = input.Title;
            threadModel.UpdatedAt = DateTime.UtcNow;
            _threadRepository.Update(threadModel);

            await _uow.SaveChangesAsync();
        }

        public async Task<ThreadContentDetails> UpdateThreadContents(ThreadUpdateContents input)
        {
            var threadModel = _threadRepository.Find(input.Id);
            var latestContent = _threadContentRepository.GetAll()
                .Where(x => x.ThreadId == input.Id)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();

            if (latestContent.Content == input.Contents)
                throw new Exceptions.BadRequestException("NOTHING_CHANGED");

            var threadContentModel = new Entities.ThreadContent
            {
                Id = Guid.NewGuid(),
                ThreadId = input.Id,
                CreatedAt = DateTime.UtcNow,
                Content = input.Contents,
            };
            _threadContentRepository.Insert(threadContentModel);

            threadModel.UpdatedAt = DateTime.UtcNow;
            _threadRepository.Update(threadModel);

            await _uow.SaveChangesAsync();

            return new ThreadContentDetails
            {
                Id = threadContentModel.Id,
                CreatedAt = threadContentModel.CreatedAt,
                Contents = threadContentModel.Content,
                ThreadId = threadContentModel.ThreadId
            };
        }

        public async Task UpdateThreadTags(ThreadUpdateTags input)
        {
            var threadModel = _threadRepository.Find(input.Id);
            var tags = _threadRepository.GetAll().Where(x => x.Id == input.Id)
                .SelectMany(x => x.Tags).ToList();

            // tags
            if (input.Tags != null && input.Tags.Length > 0)
            {
                // remove removed tags
                tags.RemoveAll(x => input.Tags.Select(y => y.Id).Contains(x.TagId) == false);

                // add new existing tags
                var newExistingTags = input.Tags.Where(x => x.Id.HasValue && tags.Select(y => y.TagId).Contains(x.Id.Value) == false);
                tags.AddRange(newExistingTags.Select(x => new Entities.ThreadTag
                {
                    Id = Guid.NewGuid(),
                    TagId = x.Id.Value,
                }));

                // add new not-existing tags
                var newNotExistingTags = input.Tags.Where(x => x.Id.HasValue == false);
                var newNotExistingTagEntities = newNotExistingTags
                    .Select(x => new Entities.Tag { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = x.Name })
                    .ToArray();
                _tagRepository.BatchInsert(newNotExistingTagEntities);
                tags.AddRange(newNotExistingTagEntities.Select(x => new Entities.ThreadTag
                {
                    Id = Guid.NewGuid(),
                    TagId = x.Id,
                }));
            }
            else
            {
                tags.RemoveAll(x => true);
            }

            threadModel.Tags = tags;
            threadModel.UpdatedAt = DateTime.UtcNow;
            _threadRepository.Update(threadModel);

            await _uow.SaveChangesAsync();
        }

        public async Task DeleteThread(Guid id)
        {
            var model = _threadRepository.Find(id);
            _threadRepository.Remove(model);

            await _uow.SaveChangesAsync();
        }


        public ICollection<ThreadContentBrief> GetAllContents(Guid threadId)
        {
            return _threadContentRepository.GetAll()
                .Where(x => x.ThreadId == threadId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ThreadContentBrief
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                }).ToList();
        }

        public ThreadContentDetails GetContent(Guid id)
        {
            return _threadContentRepository.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new ThreadContentDetails
                {
                    Id = x.Id,
                    ThreadId = x.ThreadId,
                    CreatedAt = x.CreatedAt,
                    Contents = x.Content
                }).FirstOrDefault();
        }


        public ICollection<TagDetails> GetAllTags()
        {
            return _tagRepository.GetAll()
                .OrderBy(x => x.Name)
                .Select(x => new TagDetails
                {
                    Id = x.Id,
                    Name = x.Name,
                    ThreadsCount = x.Threads.Count
                }).ToList();
        }

        public ICollection<TagBrief> GetThreadTags(Guid id)
        {
            return _threadRepository.GetAll()
                .Where(x => x.Id == id)
                .SelectMany(x => x.Tags)
                .Select(x => new TagBrief
                {
                    Id = x.TagId,
                    Name = x.Tag.Name,
                })
                .OrderBy(x => x.Name)
                .ToList();
        }
    }
}
