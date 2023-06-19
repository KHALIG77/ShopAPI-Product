using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Api.DTOs.ProductDTOs
{
    
    public class ProductPostDTO
    {
        
        public int BrandId { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DiscountPercent { get; set; }
       
        public IFormFile ProductImage { get; set; }
    }
    public class ProductPostDTOValidation:AbstractValidator<ProductPostDTO>

    {
        public ProductPostDTOValidation()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(20).MinimumLength(5);
            RuleFor(x => x.SalePrice).GreaterThanOrEqualTo(x => x.CostPrice).NotEmpty();
            RuleFor(x => x.CostPrice).GreaterThanOrEqualTo(0).NotEmpty();
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.DiscountPercent > 0)
                {
                    var price = x.SalePrice * (100 - x.DiscountPercent) / 100;
                    if (x.CostPrice > price)
                    {
                        context.AddFailure(nameof(x.DiscountPercent), "DiscountPercent incorrect");
                    }

                }
            });
            RuleFor(x => x.ProductImage.FileName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Image is required")
            .Must(fileName =>
            {
                string extension = Path.GetExtension(fileName);
                return extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase);


            }).WithMessage("Extention will be jpg");

            RuleFor(x => x.ProductImage.Length).Cascade(CascadeMode.Stop).LessThanOrEqualTo(2*1024*1024).WithMessage("File size will be max 2mb");

        }

    }
}
