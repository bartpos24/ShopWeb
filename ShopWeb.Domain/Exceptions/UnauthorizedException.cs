using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Domain.Exceptions
{
	public class UnauthorizedException : ApiException
	{
		public UnauthorizedException(string message = "You are not authorized to perform this action.")
			: base(message, 401, "UNAUTHORIZED") { }
	}
}
