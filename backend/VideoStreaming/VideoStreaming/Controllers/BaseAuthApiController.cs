using Microsoft.AspNetCore.Authorization;
using VideoStreaming.Models;

namespace VideoStreaming.Controllers
{
    [Authorize]
    public class BaseAuthApiController : BaseApiController
    {
        public User CurrentUser { get; set; } = default!;
    }
}
