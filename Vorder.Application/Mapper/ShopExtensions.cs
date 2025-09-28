using Vorder.Application.DTOs.Shop;
using Vorder.Domain.Entities;

namespace Vorder.Application.Mapper
{
    public static class ShopExtensions
    {
        public static Shop ToShop(this CreateShopDTO dto, Guid ownerId)
        {
            return new Shop
            {
                ShopId = Guid.NewGuid(),
                ShopName = dto.ShopName,
                Theme = dto.Theme,
                OwnerId = ownerId,
                IsActive = true,
                CreationTime = DateTime.UtcNow,
                CreatorId = ownerId,
                IsDeleted = false
            };
        }
        public static ReturnShopDTO ToShop(this Shop shop, string ownerName)
        {
            return new ReturnShopDTO
            {
                ShopId = shop.ShopId,
                ShopName = shop.ShopName,
                Theme = shop.Theme,
                OwnerName = ownerName,
                IsActive = shop.IsActive,
                CreationTime = shop.CreationTime,
                CreatorId = shop.CreatorId,
                IsDeleted = shop.IsDeleted
            };
        }
    }
}
