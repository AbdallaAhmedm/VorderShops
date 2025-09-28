using Vorder.Infrastructure.Data;

namespace Vorder.Domain.Entities
{
    public class Shop : BaseProperties
    {
        public Guid ShopId { get; set; }

        public required string ShopName { get; set; }

        public Guid OwnerId { get; set; }

        public string? Theme { get; set; }

        public bool IsActive { get; set; }

        public ApplicationUser Owner { get; set; }

    }
}
