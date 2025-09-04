namespace Domain.Entities.Common
{
    public class DomainException : Exception
    {
        public int StatusCode { get; }
        public DomainException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
