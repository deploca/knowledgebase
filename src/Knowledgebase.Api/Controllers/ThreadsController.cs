using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Knowledgebase.Models.Thread;

namespace Knowledgebase.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThreadsController : ControllerBase
    {
        private readonly Application.Services.ThreadService _threadService;
        public ThreadsController(
            Application.Services.ThreadService threadService)
        {
            _threadService = threadService;
        }

        [HttpGet]
        public IActionResult GetThreads(Guid? category_id, string keyword)
        {
            var data = _threadService.GetAll(new ThreadSearch
            {
                CategoryId = category_id,
                Keyword = keyword
            });
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult GetSingleThreads(Guid id)
        {
            var data = _threadService.GetDetails(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> PostThread([FromBody] ThreadCreate input)
        {
            var result = await _threadService.Create(input);
            return Ok(result);
        }
    }
}
