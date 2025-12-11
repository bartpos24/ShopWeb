using Microsoft.AspNetCore.Http;
using ShopWeb.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Extensions
{
    public interface IApiExceptionHandler
    {
        //ApiResult<T> HandleException<T>(Exception ex);
        //Task<ApiResult<T>> ExecuteAsync<T>(Func<Task<T>> apiCall);
        //Task<ApiResult> ExecuteAsync(Func<Task> apiCall);
    }
}
