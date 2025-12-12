using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Domain.Exceptions
{
	public class NotFoundException : ApiException
	{
		public NotFoundException(string message, string? errorCode = null)
			: base(message, 404, errorCode) { }

		public NotFoundException(string entityName, object key)
			: base($"{entityName} with key '{key}' was not found.", 404, "ENTITY_NOT_FOUND") { }
	}
}
