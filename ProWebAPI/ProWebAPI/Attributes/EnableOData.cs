using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.OData;
using Newtonsoft.Json;
using ProWebAPI.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProWebAPI.Attributes
{
    public class EnableODataAttribute : EnableQueryAttribute
    {
        public override void ValidateQuery(HttpRequest request, ODataQueryOptions queryOptions)
        {
            try
            {
                base.ValidateQuery(request, queryOptions);
            }
            catch (ODataException ex)
            {
                var errorResponse = new ErrorResponse
                {
                    ErrorCode = ErrorCodes.ERR02.ToString(),
                    Message = "Unsupported OData query",
                    Info = new List<string>() { ex.Message },
                    Status = ResponseStatus.WARNING.ToString()
                };

                request.HttpContext.Response.ContentType = "application/json";
                request.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                request.HttpContext.Response.WriteAsJsonAsync(errorResponse);
                return;
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    ErrorCode = ErrorCodes.ERR03.ToString(),
                    Message = "Something went wrong",
                    Status = ResponseStatus.FAILED.ToString(),
                    Info = new List<string>() { "An operation on the server resulted in failure" }
                };
                request.HttpContext.Response.ContentType = "application/json";
                request.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                request.HttpContext.Response.WriteAsJsonAsync(errorResponse);
                return;
            }
        }
    }
}