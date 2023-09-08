using System.Net;

namespace VideoStreaming.Exceptions
{
    public class TerminateRequestException : Exception
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
        public Dictionary<string, string[]>? FieldsErrors;

        public TerminateRequestException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public TerminateRequestException(string fieldName, string errorMessage)
            : this("One or more validation errors occurred.")
        {
            FieldsErrors = new()
            {
                { fieldName, new string[] { errorMessage } }
            };
        }

        public TerminateRequestException(string message, Exception? inner)
            : base(message, inner)
        { }
    }
}
