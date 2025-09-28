using Microsoft.AspNetCore.Identity;
using Vorder.Domain.Entities;

namespace Vorder.Infrastructure.Data
{

    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid? ShopID { get; set; }
        public Shop? Shop { get; set; }
    }
}
