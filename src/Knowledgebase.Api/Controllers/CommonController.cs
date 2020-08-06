using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Knowledgebase.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonController : ControllerBase
    {
        private readonly UtilityServices.IAppSession _session;
        private readonly Application.Services.AppSettingService _appSettingService;
        public CommonController(UtilityServices.IAppSession session,
            Application.Services.AppSettingService appSettingService)
        {
            _session = session;
            _appSettingService = appSettingService;
        }

        [HttpGet("app-settings")]
        public IActionResult GetAppSettings()
        {
            Console.WriteLine("Current Scheme = " + Request.Scheme);
            var data = _appSettingService.GetAll();
            return Ok(data);
        }

        [Authorize]
        [HttpGet("user-info")]
        public IActionResult UserInfo()
        {
            var data = _session.GetCurrentUserInfo();
            return Ok(data);
        }
    }
}
