using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
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

                request.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                request.HttpContext.Response.WriteAsJsonAsync(errorResponse);
                return;
            }
        }
    }
}