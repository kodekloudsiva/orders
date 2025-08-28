namespace orders.domain.Common
{
    public class DomainResult<T> //where T : BaseEntity
    {
        public T? Value { get; }
        public List<DomainError> Errors { get; }
        public bool IsFailure => Errors.Count != 0;

        public bool IsSuccess => Errors.Count == 0;

        private DomainResult(T value)
        {
            Value = value;
            Errors = new List<DomainError>();
        }

        private DomainResult(List<DomainError> errors)
        {
            Errors = errors;
        }

        public static DomainResult<T> Success(T entity) => new(entity);
        public static DomainResult<T> Failure(DomainError error) => new(new List<DomainError> { error });
        public static DomainResult<T> Failure(List<DomainError> errors) => new(errors);
    }

    public record DomainError(string Code, string Message, string? Field = null);

    
}
