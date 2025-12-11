using AutoMapper;
using ShopWeb.Application.Mapping;
using System.ComponentModel;

namespace ShopWeb.Application.TransferObjects.Inventory
{
    public class InventoryVm
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
		[DisplayName("Nazwa i adres firmy")]
		public string CompanyInformation { get; set; }
		[DisplayName("Skład komisji inwentaryzacyjnej")]
		public List<string> CommissionTeam { get; set; }
		[DisplayName("Wycenił")]
		public string PersonToValue { get; set; }
		[DisplayName("Sprawdził")]
		public string PersonToCheck { get; set; }
		[DisplayName("Data utworzenia")]
		public DateTime CreatedAt { get; set; }
		[DisplayName("Data rozpoczęcia")]
		public DateTime StartDate { get; set; }
		[DisplayName("Data zakończenia")]
		public DateTime? EndDate { get; set; }
		[DisplayName("Utworzony przez")]
		public string CreatedByUser { get; set; }
		[DisplayName("Status")]
		public string Status { get; set; }

		// My first attempt at mapping
		//public void Mapping(Profile profile)
		//{
		//	profile.CreateMap<ShopWeb.Domain.Models.Inventory, InventoryVm>()
		//		.ForMember(d => d.CreatedByUser, opt => opt.MapFrom(s => s.CreatedByUser != null ? $"{s.CreatedByUser.Name} {s.CreatedByUser.Surname}" : string.Empty))
		//		.ForMember(d => d.Status, opt => opt.MapFrom(s => s.InventoryStatus != null ? s.InventoryStatus.Name : string.Empty))
		//		.ForMember(d => d.CommissionTeam, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.ComissionTeam) ? new List<string>() :
		//			s.ComissionTeam.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList()))
		//  .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
		//  .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name ?? string.Empty))
		//  .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Type ?? string.Empty))
		//  .ForMember(d => d.ExecuteWay, opt => opt.MapFrom(s => s.ExecuteWay ?? string.Empty))
		//  .ForMember(d => d.ResponsiblePerson, opt => opt.MapFrom(s => s.ResponsiblePerson ?? string.Empty))
		//  .ForMember(d => d.CompanyInformation, opt => opt.MapFrom(s => $"{s.CompanyName ?? ""}, {s.CompanyAddress ?? ""}"))
		//  .ForMember(d => d.PersonToValue, opt => opt.MapFrom(s => s.PersonToValue ?? string.Empty))
		//  .ForMember(d => d.PersonToCheck, opt => opt.MapFrom(s => s.PersonToCheck ?? string.Empty))
		//  .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt))
		//  .ForMember(d => d.StartDate, opt => opt.MapFrom(s => s.StartDate));
		//}
	}
}
