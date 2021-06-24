using Microsoft.AspNet.OData;
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
    public class StudentsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        private readonly IProjectService projectService;

        public StudentsController(IConfiguration configuration, IUserService userService, IProjectService projectService)
        {
            this.configuration = configuration;
            this.userService = userService;
            this.projectService = projectService;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [Route("Success")]
        public Response Success(int id)
        {
            var user = userService.GetUserById(id);
            if (user.IsSuccess)
            {
                return new SuccessResponse<ProWeb.Entities.User>
                {
                    Data = user.Data
                };
            }
            else
            {
                return new ErrorResponse
                {
                    Info = user.Messages.ToList()
                };
            }
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        [Route("Success")]
        [EnableOData]
        public IActionResult Success20()
        {
            var listOfData = new List<Student>();
            listOfData.Add(new Student { Name = "Student A", Age = 10 });
            listOfData.Add(new Student { Name = "Student B", Age = 11 });
            listOfData.Add(new Student { Name = "Student C", Age = 12 });
            listOfData.Add(new Student { Name = "Student D", Age = 13 });
            return Ok(new SuccessResponse<List<Student>>
            {
                Data = listOfData
            });
        }

        [HttpGet]
        [Route("Failure")]
        public Response Failure()
        {
            return new ErrorResponse
            {
                ErrorCode = ErrorCodes.ERR01.ToString(),
                Message = "Some general purpose error message",
                Info = new List<string>
                {
                    { "Suggession 1" },
                    { "Suggession 2" }
                }
            };
        }

        [HttpPost]
        [Route("Submit")]
        public Response Submit(User user)
        {
            return new SuccessResponse<string>
            {
                Data = "Data successfully submitted"
            };
        }

        [HttpGet]
        [Route("GetSettings")]
        public Response GetSettings()
        {
            var settings = configuration.GetSection("ConnectionString").Value;
            return new SuccessResponse<object>
            {
                Data = new
                {
                    ConnectionString = settings
                }
            };
        }
    }
}