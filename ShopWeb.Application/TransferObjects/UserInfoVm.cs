using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.TransferObjects
{
	public class UserInfoVm
	{
		public string UserId { get; set; }
		public string Username { get; set; }
		public string GivenName { get; set; }
		public string FamilyName { get; set; }
		public string FullName => $"{GivenName} {FamilyName}";
		public string Email { get; set; }
		public List<string> Roles { get; set; } = new();
		public string LoginType { get; set; }
		public string IpAddress { get; set; }
	}
}
