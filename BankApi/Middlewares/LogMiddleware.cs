using Microsoft.AspNetCore.Http.Extensions;

namespace BankApi.Middlewares
{
    public class LogMiddleware : IMiddleware
    {
        public Serilog.ILogger Logger { get; }

        public LogMiddleware(
            Serilog.ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            async Task callNext(HttpContext ctx)
            {
                try
                {
                    await next.Invoke(ctx);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Unhandled Error {Method} {Url}",
                        new Dictionary<string, object>
                        {
                            { "Method", context.Request.Method.ToString() },
                            { "Url", context.Request.GetDisplayUrl() },
                        });
                    throw;
                }
            }

            LogRequestAsync(context.Request);
            await LogResponseAsync(context, callNext);
        }
        private void LogRequestAsync(HttpRequest request)
        {
            Logger.Information("Received request {Method} {Url}",
                request.Method,
                request.GetDisplayUrl());
        }

        private async Task LogResponseAsync(HttpContext context, RequestDelegate next)
        {   
            await next(context);

            Logger.Information("Sending response {Status} {Url}",
                context.Response.StatusCode,
                context.Request.GetDisplayUrl());
        }
    }
}
