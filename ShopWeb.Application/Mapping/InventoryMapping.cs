using AutoMapper;
using ShopWeb.Application.TransferObjects.Inventory;
using ShopWeb.Domain.Models;
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
					s.ComissionTeam != null && s.ComissionTeam.Any()
						? string.Join(";", s.ComissionTeam)
						: string.Empty))
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => 0)) // Don't map Id for new entities
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.EndDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => 0)) // Should be set from current user
                .ForMember(dest => dest.InventoryStatusId, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.InventoryStatus, opt => opt.Ignore());

			CreateMap<CommonInventoryPositionVm, CommonInventoryPosition>()
				.ForMember(d => d.UserId, opt => opt.MapFrom(s => 0))
				.ForMember(d => d.Inventory, opt => opt.Ignore())
				.ForMember(d => d.Unit, opt => opt.Ignore())
				.ForMember(d => d.User, opt => opt.Ignore())
				.ForMember(d => d.ModifiedByUser, opt => opt.Ignore());

			CreateMap<SummaryInventoryPosition, SummaryInventoryPositionVm>()
				.ForMember(d => d.ScanDate, opt => opt.MapFrom(s => s.DateOfScanOrModification));

        }
	}
}
