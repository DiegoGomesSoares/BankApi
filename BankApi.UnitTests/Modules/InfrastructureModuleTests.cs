using BankApi.Domain.Interfaces;
using BankApi.Infrastructure.Repository.Readers;
using BankApi.Infrastructure.Repository.Writers;
using BankApi.UnitTests.AutoData;
using FluentAssertions;
using System;
using Xunit;

namespace BankApi.UnitTests.Modules
{
    public class InfrastructureModuleTests : IClassFixture<DependencyInjectionClassFixture>
    {
        private readonly IServiceProvider _provider;
        public InfrastructureModuleTests(
            DependencyInjectionClassFixture fixture)
        {
            _provider = fixture.Provider;
        }

        [Fact]
        public void GetService_IEventProcessorStrategy_Correctly()
        {
            _provider.GetService(typeof(IAccountReader))
                                 .Should().BeOfType<AccountReader>();
        }

        [Fact]
        public void GetService_IAccountWriter_Correctly()
        {
            _provider.GetService(typeof(IAccountWriter))
                                 .Should().BeOfType<AccountWriter>();
        }
    }
}
