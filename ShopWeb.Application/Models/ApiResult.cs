using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Models
{
    public class ApiResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public int? StatusCode { get; set; }
        public Dictionary<string, string[]>? ValidationErrors { get; set; }

        public static ApiResult<T> Success(T data)
        {
            return new ApiResult<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = 200
            };
        }

        public static ApiResult<T> Failure(string errorMessage, int statusCode = 500, Dictionary<string, string[]>? validationErrors = null)
        {
            return new ApiResult<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode,
                ValidationErrors = validationErrors
            };
        }
    }

    public class ApiResult : ApiResult<object>
    {
        public static new ApiResult Success()
        {
            return new ApiResult
            {
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public static new ApiResult Failure(string errorMessage, int statusCode = 500, Dictionary<string, string[]>? validationErrors = null)
        {
            return new ApiResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode,
                ValidationErrors = validationErrors
            };
        }
    }
}
