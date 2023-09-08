using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using VideoStreaming.Persistence;
using VideoStreaming.Controllers;
using Microsoft.EntityFrameworkCore;

namespace VideoStreaming.Filters
{
    public class BaseAuthApiControllerInitializationActionFilter : IAsyncActionFilter
    {
        private readonly VideoStreamingDbContext _context;
        private readonly ILogger<BaseAuthApiControllerInitializationActionFilter> _logger;

        public BaseAuthApiControllerInitializationActionFilter(
            VideoStreamingDbContext context,
            ILogger<BaseAuthApiControllerInitializationActionFilter> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as BaseAuthApiController;

            if (controller == null)
            {
                await next();
                return;
            }

            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                var accessTokenHeaderValue = context.HttpContext.Request.Headers.Authorization;
                _logger.LogWarning(
                    "UserId is null in the authorization token. Path: {0}. Token: {1}",
                    context.HttpContext.Request.Path,
                    accessTokenHeaderValue);

                context.Result = GetProblemDetailsResult("Неверный идентификатор пользователя");
                return;
            }

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == userId);
            if (user == null)
            {
                _logger.LogWarning("Cannot find a user. UserId: {0}", userId);

                context.Result = GetProblemDetailsResult("Невозможно найти пользователя");
                return;
            }

            controller.CurrentUser = user;

            await next();
        }

        private ObjectResult GetProblemDetailsResult(string title)
        {
            var details = new ProblemDetails()
            {
                Title = title,
                Status = (int)HttpStatusCode.BadRequest,
            };

            var result = new ObjectResult(details)
            {
                StatusCode = details.Status
            };

            return result;
        }
    }
}
