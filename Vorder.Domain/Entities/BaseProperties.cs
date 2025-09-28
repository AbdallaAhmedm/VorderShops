using Vorder.Infrastructure.Data;

namespace Vorder.Domain.Entities
{
    public class BaseProperties
    {
        public bool IsDeleted { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? LastModifierId { get; set; }
        public ApplicationUser LastModifier { get; set; }
        public ApplicationUser Creator { get; set; }
    }
}
