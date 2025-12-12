using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Domain.Exceptions
{
	public class ServiceUnavailableException : ApiException
	{
		public ServiceUnavailableException(string message = "Service is temporarily unavailable. Please try again later.")
			: base(message, 503, "SERVICE_UNAVAILABLE") { }
	}
}
