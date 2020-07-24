using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Knowledgebase.Models.Administration;

namespace Knowledgebase.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly Application.Services.AdministrationService _administrationService;
        public AdminController(Application.Services.AdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Filters.Transactional]
        public IActionResult Setup([FromBody] SetupRequest input)
        {
            _administrationService.Setup(input);
            return Ok();
        }
    }
}
