using AutoFixture.Idioms;
using BankApi.Controllers;
using BankApi.Domain.Entities;
using BankApi.UnitTests.AutoData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace BankApi.UnitTests.Controllers
{
    public class BalanceControllerTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
            => assertion.Verify(typeof(BalanceController).GetConstructors());

        [Theory, AutoNSubstituteData]
        public async Task GetBalanceByAccountIdAsync_WhenAccountIsNull_ShouldReturnNotFound(
            string accountId,
            BalanceController sut)
        {
            sut.AccountReader.GetByIdAsyn(Arg.Any<string>()).Returns((Account)null);

            var actual = await sut.GetBalanceByAccountIdAsync(accountId) as NotFoundObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            actual.Value.Should().Be(0);

            await sut.AccountReader.Received().GetByIdAsyn(accountId);
        }

        [Theory, AutoNSubstituteData]
        public async Task GetBalanceByAccountIdAsync_WhenAccountIsNull_ShouldReturnOkWithBalance(
            string accountId,
            Account account,
            BalanceController sut)
        {
            sut.AccountReader.GetByIdAsyn(Arg.Any<string>()).Returns(account);

            var actual = await sut.GetBalanceByAccountIdAsync(accountId) as OkObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(account.Balance);

            await sut.AccountReader.Received().GetByIdAsyn(accountId);
        }
    }
}
