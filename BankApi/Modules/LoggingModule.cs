using Serilog;

namespace BankApi.Modules
{
    public static class LoggingModule
    {
        public static IServiceCollection AddLoggingModule(
           this IServiceCollection services)
        {
            services.AddSingleton<Serilog.ILogger>(ctx =>
            {
                var logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger();

                return logger;
            });

            return services;
        }
    }
}
