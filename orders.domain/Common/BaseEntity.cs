namespace orders.domain.Common
{
    public abstract class BaseEntity
    {
        public DateTime CreatedOn { get; protected set; }
        public DateTime LastModifiedOn { get; protected set; }

        protected void UpdateLastModified() => LastModifiedOn = DateTime.UtcNow;
    }
}
