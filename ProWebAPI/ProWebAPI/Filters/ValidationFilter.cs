using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProWebAPI.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProWebAPI.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errorResponse = new ErrorResponse
                {
                    ErrorCode = ErrorCodes.ERR01.ToString(),
                    Message = "Data submitted is not in correct format",
                    Status = ResponseStatus.WARNING.ToString()
                };

                var errorModalState = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kv => kv.Key, kv => kv.Value.Errors.Select(x => x.ErrorMessage))
                    .ToArray();

                foreach (var error in errorModalState)
                {
                    foreach (var subError in error.Value)
                    {
                        errorResponse.Info.Add($"{error.Key}: {subError}");
                    }
                }

                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }
            await next();
        }
    }
}