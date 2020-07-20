using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Knowledgebase.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonController : ControllerBase
    {
        private readonly Application.Services.AppSettingService _appSettingService;
        public CommonController(Application.Services.AppSettingService appSettingService)
        {
            _appSettingService = appSettingService;
        }

        [HttpGet("app-settings")]
        public IActionResult GetAppSettings()
        {
            var data = _appSettingService.GetAll();
            return Ok(data);
        }
    }
}
