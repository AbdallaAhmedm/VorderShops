using Vorder.Application.DTOs.Shop;
using Vorder.Domain.Entities;

namespace Vorder.Application.Interfaces.Repositories
{
    public interface IShopRepository : IGenericRepository<Shop>
    {

        Task<Shop> AddShopAsync(CreateShopDTO entity, Guid userID);
    }
}
