using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blog.Domain.Models.Shared
{
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public Result(TValue value)
        {
            _value = value;
        }

        protected Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        protected Result(bool isSuccess, Error error) : base(isSuccess, error)
        {}

        public TValue Value => _value!;

        public static Result<TValue> Create(TValue value, bool isSuccess, Error error) => new(value, isSuccess, error);

        public static implicit operator Result<TValue>(TValue? value) => Create(value!, true, Error.None);
    }
}
