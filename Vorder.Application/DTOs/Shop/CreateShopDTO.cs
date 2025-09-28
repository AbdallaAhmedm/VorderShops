namespace Vorder.Application.DTOs.Shop
{
    public class CreateShopDTO
    {
        public required string ShopName { get; set; }

        public string? Theme { get; set; }
    }
}
