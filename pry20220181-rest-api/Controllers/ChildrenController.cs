﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Services;

namespace pry20220181_rest_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("children")]
    public class ChildrenController : ControllerBase
    {
        private readonly IChildService _childService;

        public ChildrenController(IChildService childService)
        {
            _childService = childService;
        }

        [HttpGet(Name ="GetChildByDni")]
        public async Task<ChildDTO> GetByDni([FromQuery] string dni)
        {
            var child = await _childService.GetChildByDniAsync(dni);
            return child;
        }
    }
}