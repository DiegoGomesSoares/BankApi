using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BankApi.UnitTests.AutoData
{
    public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("tests");

            builder.ConfigureAppConfiguration((ctx, builder) =>
            {
                builder.AddConfiguration(new ConfigurationBuilder()
                       .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json")
                       .Build());
            });

            return base.CreateHost(builder);
        }
    }
}
