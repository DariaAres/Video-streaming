using Microsoft.AspNetCore.Mvc;
using VideoStreaming.Filters;

namespace VideoStreaming.Extensions
{
    public static class MvcOptionsExtensions
    {
        public static void AddDefaultFilters(this MvcOptions options)
        {
            options.Filters.Add(typeof(RollbackTransactionExceptionFilter));
            options.Filters.Add(typeof(TerminateRequestExceptionsFilter));
            options.Filters.Add(typeof(BaseAuthApiControllerInitializationActionFilter));
        }
    }
}
