using Vorder.Application.DTOs.Shop;
using Vorder.Application.Interfaces.Repositories;
using Vorder.Application.Mapper;
using Vorder.Domain.Entities;

namespace Vorder.Infrastructure.Data.Repositories
{
    public class ShopRepository : GenericRepository<Shop>, IShopRepository
    {
        public ShopRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Shop> AddShopAsync(CreateShopDTO entity, Guid userId)
        {
            Shop shop = entity.ToShop(userId);

            await _context.Shops.AddAsync(shop);

            var owner = await _context.Users.FindAsync(userId);
            if (owner != null)
            {
                owner.ShopID = shop.ShopId;
                _context.Users.Update(owner);
            }
            _context.SaveChanges();
            return shop;
        }
    }
}
