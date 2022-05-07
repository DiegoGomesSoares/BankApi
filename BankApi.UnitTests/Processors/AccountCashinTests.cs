using AutoFixture.Idioms;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Processors;
using BankApi.UnitTests.AutoData;
using FluentAssertions;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace BankApi.UnitTests.Processors
{
    public class AccountCashinTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardClauses(
            GuardClauseAssertion assertion)
            => assertion.Verify(typeof(AccountCashin).GetConstructors());

        [Theory, AutoNSubstituteData]
        public void Sut_ShouldImplementProperlyInterface(
            AccountCashin sut)
            => sut.Should().BeAssignableTo<IAccountCashin>();

        [Theory, AutoNSubstituteData]
        public async Task Cashin_ShouldUpdateBalanceAccount(
            Account account,
            decimal amount,
            AccountCashin sut)
        {
            var expectedBalance = account.Balance + amount;

            sut.AccountWriter.UpdateBalance(Arg.Do<Account>(x =>
            {
                x.Id.Should().Be(account.Id);
                x.Balance.Should().Be(expectedBalance);
            }));

            await sut.Cashin(account, amount);
            await sut.AccountWriter.Received().UpdateBalance(Arg.Any<Account>());
        }
    }
}
