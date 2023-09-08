using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VideoStreaming.Exceptions;

namespace VideoStreaming.Filters
{
    public class TerminateRequestExceptionsFilter : IExceptionFilter
    {
        private readonly ILogger<TerminateRequestExceptionsFilter> _logger;

        public TerminateRequestExceptionsFilter(ILogger<TerminateRequestExceptionsFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An exception ocurred while processing the request");

            ProblemDetails result = default!;
            if (context.Exception is TerminateRequestException)
            {
                result = OnBadRequestException(context);
            }
            else
            {
                result = OnAnyException(context);
            }

            context.ExceptionHandled = true;

            context.Result = new ObjectResult(result)
            {
                StatusCode = result.Status
            };
        }

        private ProblemDetails OnBadRequestException(ExceptionContext context)
        {
            var exception = (TerminateRequestException)context.Exception;

            var problemDetails = new ProblemDetails
            {
                Title = exception.Message,
                Status = (int)exception.StatusCode,
                Instance = context.HttpContext.Request.Path
            };

            var fieldErrors = exception.FieldsErrors;
            if (fieldErrors != null)
            {
                problemDetails.Extensions.Add("errors", fieldErrors);
            }

            return problemDetails;
        }

        private ProblemDetails OnAnyException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception ocurred");

            var problemDetails = new ProblemDetails
            {
                Title = "Произошла ошибка",
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = context.HttpContext.Request.Path
            };

            return problemDetails;
        }
    }
}
