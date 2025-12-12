using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Domain.Infrastructure
{
	public class ApiErrorResponse
	{
		public string? Message { get; set; }
		public string? ErrorCode { get; set; }
		public IDictionary<string, string[]>? Errors { get; set; }
		public string? TraceId { get; set; }
	}
}