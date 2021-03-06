﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Knowledgebase.Models;
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
        public IActionResult GetThreads(Guid? category_id, Guid? tag_id, string keyword)
        {
            var data = _threadService.GetAll(new ThreadSearch
            {
                TagId = tag_id,
                CategoryId = category_id,
                Keyword = keyword
            });
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult GetSingleThread(Guid id)
        {
            var data = _threadService.GetDetails(id);
            return Ok(data);
        }

        [HttpPost]
        [Filters.Transactional]
        public IActionResult CreateThread([FromBody] ThreadCreate input)
        {
            var result = _threadService.Create(input);
            return Ok(result);
        }

        [HttpPut]
        [Filters.Transactional]
        public IActionResult UpdateThread([FromBody] ThreadUpdate input)
        {
            _threadService.UpdateThread(input);
            return Ok();
        }

        [HttpPut("title")]
        [Filters.Transactional]
        public IActionResult UpdateThreadTitle([FromBody] ThreadUpdateTitle input)
        {
            _threadService.UpdateThreadTitle(input);
            return Ok();
        }

        [HttpPut("tags")]
        [Filters.Transactional]
        public IActionResult UpdateThreadTags([FromBody] ThreadUpdateTags input)
        {
            _threadService.UpdateThreadTags(input);
            return Ok();
        }

        [HttpPut("contents")]
        [Filters.Transactional]
        public IActionResult UpdateThreadContents([FromBody] ThreadUpdateContents input)
        {
            var result = _threadService.UpdateThreadContents(input);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Filters.Transactional]
        public IActionResult DeleteThread(Guid id)
        {
            _threadService.DeleteThread(id);
            return Ok();
        }


        [HttpGet("{thread_id}/contents")]
        public IActionResult GetContentsOfThread(Guid thread_id)
        {
            var data = _threadService.GetAllContents(thread_id);
            return Ok(data);
        }

        [HttpGet("contents/{content_id}")]
        public IActionResult GetContentDetails(Guid content_id)
        {
            var data = _threadService.GetContent(content_id);
            return Ok(data);
        }


        [HttpGet("tags")]
        public IActionResult GetAllTags()
        {
            var result = _threadService.GetAllTags();
            return Ok(result);
        }
    }
}
