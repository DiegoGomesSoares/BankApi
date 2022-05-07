using BankApi.Creators;
using BankApi.Domain.Interfaces;
using BankApi.Processors;
using BankApi.UnitTests.AutoData;
using BankApi.Validators;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace BankApi.UnitTests.Modules
{
    public class ApplicationModuleTests : IClassFixture<DependencyInjectionClassFixture>
    {
        private readonly IServiceProvider _provider;

        public ApplicationModuleTests(
            DependencyInjectionClassFixture fixture)
        {
            _provider = fixture.Provider;
        }

        [Fact]
        public void GetService_IEventProcessorStrategy_Correctly()
        {
            var resolved = _provider.GetService(typeof(IEventProcessorStrategy))
                                .Should().BeOfType<EventProcessorStrategy>().Subject;

            resolved.Processors.Count().Should().Be(3);
        }

        [Fact]
        public void GetService_IEventProcessor_Correctly()
        {
            var implementations = _provider.GetServices<IEventProcessor>();

            implementations.Should().Contain(x => x.GetType() == typeof(DepositProcessor));
            implementations.Should().Contain(x => x.GetType() == typeof(WithdrawProcessor));
            implementations.Should().Contain(x => x.GetType() == typeof(TransferProcessor));
        }

        [Fact]
        public void GetService_ITransferProcessExecutor_Correctly()
        {
            var resolved = _provider.GetService<ITransferProcessExecutor>()
                                  .Should().BeOfType<TransferProcessExecutor>().Subject;

            resolved.DepositProcessor.Should().BeOfType<DepositProcessor>();
            resolved.WithDrawProcessor.Should().BeOfType<WithdrawProcessor>();
        }

        [Fact]
        public void GetService_IAccountCreator_Correctly()
        {
            _provider.GetService<IAccountCreator>()
                                  .Should().BeOfType<AccountCreator>();
        }

        [Fact]
        public void GetService_IAccountCashin_Correctly()
        {
            _provider.GetService<IAccountCashin>()
                                  .Should().BeOfType<AccountCashin>();
        }

        [Fact]

        public void GetService_IAccountCashout_Correctly()
        {
            _provider.GetService<IAccountCashout>()
                                  .Should().BeOfType<AccountCashout>();
        }

        [Fact]
        public void GetService_IWithdrawProcessorValidator_Correctly()
        {
            _provider.GetService<IWithdrawProcessorValidator>()
                                  .Should().BeOfType<WithdrawProcessorValidator>();
        }
    }
}