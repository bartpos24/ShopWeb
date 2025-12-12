using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Domain.Exceptions
{
	public class BadRequestException : ApiException
	{
		public BadRequestException(string message, string? errorCode = null)
			: base(message, 400, errorCode) { }

		public BadRequestException(
			string message,
			IDictionary<string, string[]> validationErrors)
			: base(message, 400, "VALIDATION_ERROR", validationErrors) { }
	}
}
