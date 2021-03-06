using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProWebAPI.Modal
{
    public enum ResponseStatus
    {
        SUCCESS,
        WARNING,
        FAILED
    }

    public enum ErrorCodes
    {
        //RESERVED
        ERR01, // => Invalid request validation (400)

        ERR02, // => Unsupported OData Query (400)
        ERR03, // => Unhandled exception on server (500)
        ERR04, // => Any default handled exceptions

        //CUSTOM CODES
        ERR05
    }

    public class Response
    {
        protected ResponseStatus ResponseStatus { get; set; }
        public string Status { get; set; }
    }

    public class SuccessResponse<T> : Response
    {
        public T Data { get; set; }

        public SuccessResponse()
        {
            ResponseStatus = ResponseStatus.SUCCESS;
            Status = ResponseStatus.SUCCESS.ToString();
        }
    }

    public class ErrorResponse : Response
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public List<string> Info { get; set; }

        public ErrorResponse()
        {
            ResponseStatus = ResponseStatus.FAILED;
            Status = ResponseStatus.FAILED.ToString();
            ErrorCode = ErrorCodes.ERR04.ToString();
            Info = new List<string>();
            Message = "Request was not successfull";
        }
    }
}