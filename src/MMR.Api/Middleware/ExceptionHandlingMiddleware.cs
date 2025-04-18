using MMR.Common;
using MMR.Common.Api.Responses;

namespace MMR.Api.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<ExceptionHandlingMiddleware>>();
            logger.LogCritical(
                LogEvents.UnhandledException,
                e,
                "An unhandled exception occured. This should never happen as everything should be wrapped into the Result type"
            );

            await TypedResults
                .InternalServerError(ProblemResponse.ServerError)
                .ExecuteAsync(context);
        }
    }
}