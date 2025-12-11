using AutoMapper;
using ShopWeb.Application.TransferObjects.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Mapping
{
	public class InventoryMapping : Profile
	{
		public InventoryMapping()
		{
			CreateMap<ShopWeb.Domain.Models.Inventory, InventoryVm>()
				.ForMember(d => d.CommissionTeam, opt => opt.MapFrom(s =>
					string.IsNullOrWhiteSpace(s.ComissionTeam)
						? new System.Collections.Generic.List<string>()
						: s.ComissionTeam.Split(';', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries).ToList()))
				.ForMember(d => d.CreatedByUser, opt => opt.MapFrom(s =>
					s.CreatedByUser != null
						? $"{s.CreatedByUser.Name ?? ""} {s.CreatedByUser.Surname ?? ""}".Trim()
						: string.Empty))
				.ForMember(d => d.Status, opt => opt.MapFrom(s =>
					s.InventoryStatus != null ? s.InventoryStatus.Name : string.Empty))
				.ForMember(d => d.CompanyInformation, opt => opt.MapFrom(s => $"{s.CompanyName ?? ""}, {s.CompanyAddress ?? ""}"));

			// Mapping: NewInventoryVm -> Inventory (Create/Update)
			CreateMap<NewInventoryVm, ShopWeb.Domain.Models.Inventory>()
				.ForMember(d => d.ComissionTeam, opt => opt.MapFrom(s =>
					s.CommissionTeam != null && s.CommissionTeam.Any()
						? string.Join(";", s.CommissionTeam)
						: string.Empty));
				// Properties not in NewInventoryVm - set defaults or ignore
				//.ForMember(d => d.EndDate, opt => opt.Ignore())
				//.ForMember(d => d.InventoryStatusId, opt => opt.Ignore())
				//.ForMember(d => d.CreatedByUser, opt => opt.Ignore())
				//.ForMember(d => d.InventoryStatus, opt => opt.Ignore());
		}
	}
}
