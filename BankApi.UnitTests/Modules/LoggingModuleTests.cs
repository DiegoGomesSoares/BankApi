using BankApi.UnitTests.AutoData;
using FluentAssertions;
using Serilog;
using Serilog.Core;
using System;
using Xunit;

namespace BankApi.UnitTests.Modules
{
    public class LoggingModuleTests : IClassFixture<DependencyInjectionClassFixture>
    {
        private readonly IServiceProvider _provider;
        public LoggingModuleTests(
            DependencyInjectionClassFixture fixture)
        {
            _provider = fixture.Provider;
        }

        [Fact]
        public void GetService_IEventProcessorStrategy_Correctly()
        {
            _provider.GetService(typeof(ILogger))
                                 .Should().BeOfType<Logger>();
        }

        [Fact]
        public void GetService_IEventProcessorStrategy_SameInstanceOfLogger()
        {
            Logger instance1;
            Logger instance2;

            instance1 = _provider.GetService(typeof(ILogger)).As<Logger>();
            instance2 = _provider.GetService(typeof(ILogger)).As<Logger>();

            var hash1 = instance1.GetHashCode();
            var hash2 = instance2.GetHashCode();
            hash1.Should().Be(hash2);
        }
    }
}
