using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Domain.Exceptions
{
	public class ForbiddenException : ApiException
	{
		public ForbiddenException(string message = "You don't have permission to access this resource.")
			: base(message, 403, "FORBIDDEN") { }
	}
}
