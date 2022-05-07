using BankApi.Domain.Interfaces;
using BankApi.Infrastructure;
using BankApi.Infrastructure.Repository.Readers;
using BankApi.Infrastructure.Repository.Writers;

namespace BankApi.Modules
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructureModule(
           this IServiceCollection services)
        {
            services.AddSingleton<AccountListFakeDB>();
            services.AddTransient<IAccountReader, AccountReader>();
            services.AddTransient<IAccountWriter, AccountWriter>();

            return services;
        }
    }
}
