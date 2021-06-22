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
        ERR01, // => BadRequest

        //CUSTOM CODES
        ERR02
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
            ErrorCode = ErrorCodes.ERR01.ToString();
            Info = new List<string>();
        }
    }

    public class Dto<T>
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
    }
}