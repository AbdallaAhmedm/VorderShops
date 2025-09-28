namespace Vorder.Application.DTOs
{
    public class BasePropertiesDTO
    {
        public bool IsDeleted { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? LastModifierId { get; set; }
    }
}
