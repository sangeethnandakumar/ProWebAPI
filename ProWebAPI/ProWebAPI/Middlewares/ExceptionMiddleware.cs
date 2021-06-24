using Microsoft.AspNetCore.Http;
using ProWebAPI.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProWebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //Write exception log here
                await HandleExceptionAsync(context);
            }
        }

        private Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var serverError = new ErrorResponse
            {
                ErrorCode = ErrorCodes.ERR03.ToString(),
                Message = "Something went wrong",
                Status = ResponseStatus.FAILED.ToString(),
                Info = new List<string>() { "An operation on the server resulted in failure" }
            };
            return context.Response.WriteAsJsonAsync(serverError);
        }
    }
}