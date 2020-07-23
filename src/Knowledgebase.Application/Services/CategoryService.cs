﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Knowledgebase.UnitOfWork;
using Knowledgebase.Models;
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

        public ICollection<CategoryBrief> GetAll(CategorySearch input)
        {
            var q = _categoryRepository.GetAll();
            if (input != null)
            {
                if (string.IsNullOrWhiteSpace(input.ParentId) == false)
                {
                    Guid? parentId = (input.ParentId == "root") ? (Guid?)null : Guid.Parse(input.ParentId);
                    q = q.Where(x => x.ParentCategoryId == parentId);
                }
            }

            // sort
            q = q.OrderBy(x => x.Title);

            return q.Select(x => new CategoryBrief
            {
                Id = x.Id,
                ParentCategoryId = x.ParentCategoryId,
                Title = x.Title,
                SubCategoriesCount = x.SubCategories.Count,
                ThreadsCount = x.Threads.Count,
            }).ToList();
        }

        public CategoryDetails GetDetails(Guid id)
        {
            var data = _categoryRepository.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new CategoryDetails
                {
                    Id = x.Id,
                    Title = x.Title,
                    CreatedAt = x.CreatedAt,
                    ParentCategoryId = x.ParentCategoryId,
                    HierarchyString = x.Hierarchy,
                    SubCategories = x.SubCategories.Select(y => new CategoryBrief
                    {
                        Id = y.Id,
                        Title = y.Title,
                        ParentCategoryId = y.ParentCategoryId,
                        SubCategoriesCount = y.SubCategories.Count,
                        ThreadsCount = y.Threads.Count,
                    }).OrderBy(y => y.Title).ToList(),
                    Threads = x.Threads.Select(y => new Models.Thread.ThreadBrief
                    {
                        Id = y.Id,
                        Title = y.Title,
                        CreatedAt = y.CreatedAt,
                        Tags = y.Tags.Select(z => new Models.Tag.TagBrief
                        {
                            Id = z.TagId,
                            Name = z.Tag.Name,
                        }).OrderBy(z => z.Name).ToList()
                    }).OrderBy(y => y.Title).ToList()
                }).FirstOrDefault();

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

            // calculate sibling categories
            data.SiblingCategories = _categoryRepository.GetAll()
                .Where(x => x.ParentCategoryId == data.ParentCategoryId)
                .OrderBy(x => x.Title)
                .Select(x => new IdNameDto(x.Id, x.Title)).ToList();

            return data;
        }

        public async Task<Guid> Create(CategoryCreate input)
        {
            // validation
            var hasCategoryWithSameName = _categoryRepository.GetAll()
                .Where(x => x.ParentCategoryId == input.ParentCategoryId && x.Title == input.Title).Any();
            if (hasCategoryWithSameName)
                throw new Exceptions.BadRequestException("CATEGORY_WITH_SAME_NAME_EXIST");

            // build hierarchy
            var hierarchy = "";
            if (input.ParentCategoryId.HasValue)
            {
                hierarchy = input.ParentCategoryId.Value.ToString();
                var parentCategoryHierarchy = _categoryRepository.GetAll()
                    .Where(x => x.Id == input.ParentCategoryId.Value)
                    .Select(x => x.Hierarchy).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(parentCategoryHierarchy) == false)
                    hierarchy = parentCategoryHierarchy + ";" + hierarchy;
            }

            var model = new Entities.Category
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ParentCategoryId = input.ParentCategoryId,
                Hierarchy = hierarchy,
                Title = input.Title
            };
            _categoryRepository.Insert(model);
            await _uow.SaveChangesAsync();
            return model.Id;
        }

        public async Task UpdateTitle(CategoryUpdateTitle input)
        {
            var model = _categoryRepository.Find(input.Id);

            if (model.Title == input.Title)
                throw new Exceptions.BadRequestException("NOTHING_CHANGED");

            model.Title = input.Title;
            model.UpdatedAt = DateTime.UtcNow;
            _categoryRepository.Update(model);

            await _uow.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            // check if the category has any children
            var childrenCount = _categoryRepository.GetAll()
                .Where(x => x.Id == id)
                .Select(x => x.SubCategories.Count + x.Threads.Count).FirstOrDefault();
            if (childrenCount > 0)
                throw new Exceptions.BadRequestException("CANT_DELETE_BECAUSE_HAS_CHILDREN");

            var model = _categoryRepository.Find(id);
            _categoryRepository.Remove(model);

            await _uow.SaveChangesAsync();
        }
    }
}
