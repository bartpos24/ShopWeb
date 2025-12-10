using AutoMapper;
using ShopWeb.Application.Mapping;

namespace ShopWeb.Application.TransferObjects.Inventory
{
    public class InventoryVm : IMapFrom<ShopWeb.Domain.Models.Inventory>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ExecuteWay { get; set; }
        public string ResponsiblePerson { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public List<string> CommissionTeam { get; set; }
        public string PersonToValue { get; set; }
        public string PersonToCheck { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CreatedByUser { get; set; }
        public string Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ShopWeb.Domain.Models.Inventory, InventoryVm>()
                .ForMember(d => d.CreatedByUser, opt => opt.MapFrom(s => s.CreatedByUser != null ? $"{s.CreatedByUser.Name} {s.CreatedByUser.Surname}" : string.Empty))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.InventoryStatus != null ? s.InventoryStatus.Name : string.Empty))
                .ForMember(d => d.CommissionTeam, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.ComissionTeam) ? new List<string>() : 
                    s.ComissionTeam.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList()));
        }
    }
}
