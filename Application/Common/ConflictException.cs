// Application/Common/ApplicationExceptions.cs
namespace Application.Common
{
    /// <summary>
    /// خطای قوانین دامنه و کسب‌وکار (409 Conflict)
    /// </summary>
    public sealed class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }

        public ConflictException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    /// <summary>
    /// خطای عدم یافتن موجودیت (404 NotFound)
    /// </summary>
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string entityName, object key)
            : base($"موجودیت '{entityName}' با شناسه ({key}) یافت نشد.") { }
    }
}
