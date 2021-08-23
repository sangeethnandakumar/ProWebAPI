using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProWeb.Service.Interfaces;
using ProWebAPI.Attributes;
using ProWebAPI.Modal;
using ProWebAPI.RequestDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProWebAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GoalsController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public GoalsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult GetAll()
        {
            return Ok();
        }

    }
}