namespace Blog.Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; protected set; }
        public Guid Guid { get; protected set; } = Guid.Empty;
    }
}
