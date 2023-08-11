namespace Blog.Domain.Models.Shared
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure { get => !IsSuccess; }
        public Error? Error { get; private set; }

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error is not null && !error.Equals(Error.None))
                throw new Exception("Cannot fail and suceed");

            if (!isSuccess && (error is null || error.Equals(Error.None)))
                throw new Exception("Cannot not fail and not succeed");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);
        public static Result<TValue> Failure<TValue>(Error error) => Result<TValue>.Create(default!, false, error);
    }
}
