using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Knowledgebase.Models.AppUser;

namespace Knowledgebase.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly Application.Services.AppUserService _appUserService;
        public UsersController(Application.Services.AppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpGet]
        public ICollection<AppUserBrief> GetAll()
        {
            return _appUserService.GetAll();
        }
    }
}
