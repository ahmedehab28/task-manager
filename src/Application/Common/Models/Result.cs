
namespace Application.Common.Models
{
    public class Result
    {
        internal Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        public bool Succeeded { get; }
        public string[] Errors { get; }

        public static Result Success() =>
            new(true, Array.Empty<string>());

        public static Result Failure(IEnumerable<string> errors) =>
            new(false, errors);
    }
    public class Result<T>
    {
        internal Result(bool succeeded, IEnumerable<string> errors, T? value)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
            Value = value;
        }
        public bool Succeeded { get; }
        public string[] Errors { get; }
        public T? Value { get; }
        public static Result<T> Success(T? value) => new(true, Array.Empty<string>(), value);
        public static Result<T> Failure(IEnumerable<string> errors) => new(false, errors, default);
    }
}
