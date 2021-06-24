using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProWeb.Commons
{
    public class Result<T>
    {
        public bool IsSuccess = true;
        public string[] Messages;
        public T Data;
    }

    public class Success<T> : Result<T>
    {
        public Success(T data)
        {
            Data = data;
            IsSuccess = true;
        }
    }

    public class Failure<T> : Result<T>
    {
        public Failure(params string[] messages)
        {
            Messages = messages;
            IsSuccess = false;
            Data = default(T);
        }
    }
}