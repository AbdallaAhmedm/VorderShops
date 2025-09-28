namespace Vorder.Application.DTOs.Shop
{
    public class ReturnShopDTO : BasePropertiesDTO
    {
        public Guid ShopId { get; set; }

        public required string ShopName { get; set; }

        public string OwnerName { get; set; }

        public string? Theme { get; set; }

        public bool IsActive { get; set; }

    }
}
