using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;

namespace BankApi.UnitTests.AutoData
{
    public class DependencyInjectionClassFixture
    {
        public IServiceProvider Provider { get; }

        public DependencyInjectionClassFixture()
        {
            var application = new CustomWebApplicationFactory<Program>();

            Provider = application.Services;
        }
    }

}
