using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Domain.Exceptions
{
	public class ApiException : Exception
	{
		public int StatusCode { get; }
		public string? ErrorCode { get; }
		public IDictionary<string, string[]>? ValidationErrors { get; }

		public ApiException(
			string message,
			int statusCode = 500,
			string? errorCode = null,
			IDictionary<string, string[]>? validationErrors = null)
			: base(message)
		{
			StatusCode = statusCode;
			ErrorCode = errorCode;
			ValidationErrors = validationErrors;
		}
	}
}