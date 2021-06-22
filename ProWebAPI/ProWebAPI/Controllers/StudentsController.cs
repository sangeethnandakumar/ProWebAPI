using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Route("Success")]
        public Response Success()
        {
            Thread.Sleep(1000);
            return new SuccessResponse<object>
            {
                Data = new
                {
                    FirstName = "Old",
                    LastName = "Version"
                }
            };
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        [Route("Success")]
        public Response Success20()
        {
            Thread.Sleep(1000);
            return new SuccessResponse<object>
            {
                Data = new
                {
                    FirstName = "New",
                    LastName = "Version"
                }
            };
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
    }
}