using Microsoft.AspNetCore.Mvc.Filters;
using VideoStreaming.Persistence;

namespace VideoStreaming.Filters
{
    public class RollbackTransactionExceptionFilter : IAsyncExceptionFilter
    {
        private readonly VideoStreamingDbContext _context;
        private readonly ILogger<RollbackTransactionExceptionFilter> _logger;

        public RollbackTransactionExceptionFilter(VideoStreamingDbContext context, ILogger<RollbackTransactionExceptionFilter> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var transaction = _context.Database.CurrentTransaction;

            if (transaction == null)
            {
                return;
            }

            try
            {
                await transaction.RollbackAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot rollback transaction. Trace id: {0}", context.HttpContext.TraceIdentifier);
            }
        }
    }
}
