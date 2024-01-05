using Microsoft.AspNetCore.Builder;
using Template.API.Middlewares;

namespace Template.API.Extensions;

public static class LogExtensions
{
    public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogMiddleware>();
    }
}
