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
        public IActionResult GetCategories()
        {
            //var data = new List<CategoryBrief>()
            //{
            //    new CategoryBrief { Id = Guid.NewGuid(), Title = "سیستم عامل لینوکس" },
            //    new CategoryBrief { Id = Guid.NewGuid(), Title = "آشنایی، نصب و کار با داکر" },
            //};
            var data = _categoryService.GetAll();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> PostCategory([FromBody] CategoryCreate input)
        {
            var result = await _categoryService.Create(input);
            return Ok(result);
        }
    }
}
