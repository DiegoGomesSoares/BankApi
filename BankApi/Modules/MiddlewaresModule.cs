using BankApi.Middlewares;

namespace BankApi.Modules
{
    public static class MiddlewaresModule
    {
        public static IServiceCollection AddMiddlewareModule(
            this IServiceCollection services)
        {
            services.AddTransient<LogMiddleware>();

            return services;
        }
        public static IApplicationBuilder UseMiddlewaresModule(
            this IApplicationBuilder app)
        {
            app.UseMiddleware<LogMiddleware>();

            return app;
        }
    }
}
