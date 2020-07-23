using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Knowledgebase.Models.Category;

namespace Knowledgebase.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly Application.Services.CategoryService _categoryService;
        public CategoriesController(
            Application.Services.CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetCategories(string parent_id)
        {
            var data = _categoryService.GetAll(new CategorySearch
            {
                ParentId = parent_id
            });
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryDetails(Guid id)
        {
            var data = _categoryService.GetDetails(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreate input)
        {
            var result = await _categoryService.Create(input);
            return Ok(result);
        }

        [HttpPut("title")]
        public async Task<IActionResult> UpdateThreadTitle([FromBody] CategoryUpdateTitle input)
        {
            await _categoryService.UpdateTitle(input);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _categoryService.Delete(id);
            return Ok();
        }
    }
}
