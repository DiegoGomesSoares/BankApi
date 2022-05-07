using AutoFixture.Idioms;
using BankApi.Controllers;
using BankApi.UnitTests.AutoData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace BankApi.UnitTests.Controllers
{
    public class ResetStateControllerTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
           => assertion.Verify(typeof(ResetStateController).GetConstructors());

        [Theory, AutoNSubstituteData]
        public async Task Reset_ShouldReturnOk(
            ResetStateController sut)
        {
            var actual = await sut.Reset() as OkObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(HttpStatusCode.OK.ToString());

            await sut.AccountWriter.Received().ResetAccountsAsync();
        }
    }
}
