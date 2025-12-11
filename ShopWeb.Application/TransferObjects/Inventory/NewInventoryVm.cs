using AutoMapper;
using ShopWeb.Application.Mapping;
using ShopWeb.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.TransferObjects.Inventory
{
	public class NewInventoryVm
	{
		public int Id { get; set; }
		[DisplayName("Nazwa")]
		public string Name { get; set; }
		[DisplayName("Rodzaj inwentaryzacji")]
		public string Type { get; set; }
		[DisplayName("Sposób przeprowadzania")]
		public string ExecuteWay { get; set; }
		[DisplayName("Osoba odpowiedzialna")]
		public string ResponsiblePerson { get; set; }
		[DisplayName("Nazwa firmy")]
		public string CompanyName { get; set; }
		[DisplayName("Adres firmy")]
		public string CompanyAddress { get; set; }
		[DisplayName("Skład komisji inwentaryzacyjnej")]
		public List<string> CommissionTeam { get; set; }
		[DisplayName("Wycenił")]
		public string PersonToValue { get; set; }
		[DisplayName("Sprawdził")]
		public string PersonToCheck { get; set; }
	}
}
