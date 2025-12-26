using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.TransferObjects.Inventory
{
    public class CommonInventoryPositionVm
    {
        public int Id { get; set; }
        [DisplayName("Nazwa produktu")]
        public string ProductName { get; set; }
        [DisplayName("Ilość")]
        public double Quantity { get; set; }
        [DisplayName("Cena")]
        public double Price { get; set; }
        [DisplayName("Data skanowania")]
        public DateTime ScanDate { get; set; }
        [DisplayName("Identyfikator inwentaryzacji")]
        public int InventoryId { get; set; }
        public int? ModifiedByUserId { get; set; }
        [DisplayName("Jednostka miary")]
        public int UnitId { get; set; }
        public string Unit { get; set; }
    }

    public class CommonInventoryPositionValidation : AbstractValidator<CommonInventoryPositionVm>
    {
        public CommonInventoryPositionValidation()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Nazwa produktu jest wymagana.")
                .MaximumLength(200).WithMessage("Nazwa produktu nie może przekraczać 200 znaków.")
                .MinimumLength(3).WithMessage("Nazwa produktu musi mieć co najmniej 3 znaki.");
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Ilość nie może być mniejsza od zera.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Cena nie może być mniejsza od zera.");
            RuleFor(x => x.ScanDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Data skanowania nie może być w przyszłości.");
            RuleFor(x => x.InventoryId)
                .GreaterThan(0);//.WithMessage("Identyfikator inwentaryzacji musi być większy od zera.");
            RuleFor(x => x.UnitId)
                .GreaterThan(0);//.WithMessage("Identyfikator jednostki musi być większy od zera.");
        }
    }
}
