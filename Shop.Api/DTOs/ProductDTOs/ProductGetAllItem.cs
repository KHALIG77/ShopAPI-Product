namespace Shop.Api.DTOs.ProductDTOs
{
    public class ProductGetAllItem
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public string Image { get; set; }
        public decimal DiscountPercent { get; set; }
    }
}
