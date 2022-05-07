using BankApi.Creators;
using BankApi.Domain.Interfaces;
using BankApi.Processors;
using BankApi.Validators;

namespace BankApi.Modules
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(
             this IServiceCollection services)
        {
            services.AddTransient<IEventProcessorStrategy, EventProcessorStrategy>();

            services.AddTransient<IEventProcessor, DepositProcessor>();
            services.AddTransient<IEventProcessor, WithdrawProcessor>();
            services.AddTransient<IEventProcessor, TransferProcessor>();
            
            services.AddTransient<ITransferProcessExecutor>(x =>
            {
                var depositProcessor =
                                new DepositProcessor(x.GetService<IAccountReader>(), x.GetService<IAccountCreator>(),
                                                      x.GetService<IAccountCashin>());

                var withDrawProcessor =
                                new WithdrawProcessor(x.GetService<IWithdrawProcessorValidator>(), x.GetService<IAccountCashout>(),
                                                        x.GetService<IAccountReader>());
                return new TransferProcessExecutor(withDrawProcessor, depositProcessor);
            });

            services.AddTransient<IAccountCreator, AccountCreator>();

            services.AddTransient<IAccountCashin, AccountCashin>();
            services.AddTransient<IAccountCashout, AccountCashout>();

            services.AddTransient<IWithdrawProcessorValidator, WithdrawProcessorValidator>();

            return services;
        }
    }
}
