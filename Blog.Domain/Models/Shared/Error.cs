using System.Runtime.InteropServices;

namespace Blog.Domain.Models.Shared
{
    public class Error
    {
        public int Code { get; private set; }
        public string Message { get; private set; } = string.Empty;

        private Error(int code, string message)
        {
            Code = code;
            Message = message;
        }   

        private Error() { }

        public static Error Create(int code, string message)
        {
            return new Error(code, message);
        }

        public static Error None => new();

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            var otherError = (Error)obj;

            return Code == otherError.Code && Message == otherError.Message;
        }

        public override int GetHashCode()
        {
            return 1;
        }
    }
}
