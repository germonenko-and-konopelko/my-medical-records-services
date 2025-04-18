using System.Security.Claims;
using MMR.Common.Api;

namespace MMR.Api.Middleware;

public class SetUserContextMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        Claim? currentUserClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == "user_id");
        if (currentUserClaim is null)
        {
            return next(context);
        }

        var userContext = context.RequestServices.GetService<UserContext>();
        if (userContext is not null)
        {
            userContext.CurrentUserId = currentUserClaim.Value;
        }

        return next(context);
    }
}