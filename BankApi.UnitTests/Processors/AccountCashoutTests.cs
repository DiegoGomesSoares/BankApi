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
    public class AccountCashoutTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardClauses(
            GuardClauseAssertion assertion)
            => assertion.Verify(typeof(AccountCashout).GetConstructors());

        [Theory, AutoNSubstituteData]
        public void Sut_ShouldImplementProperlyInterface(
            AccountCashout sut)
            => sut.Should().BeAssignableTo<IAccountCashout>();

        [Theory, AutoNSubstituteData]
        public async Task Cashout_ShouldUpdateBalanceAccount(
            Account account,
            decimal amount,
            AccountCashout sut)
        {
            var expectedBalance = account.Balance - amount;

            sut.AccountWriter.UpdateBalance(Arg.Do<Account>(x =>
            {
                x.Id.Should().Be(account.Id);
                x.Balance.Should().Be(expectedBalance);
            }));

            await sut.Cashout(account, amount);
            await sut.AccountWriter.Received().UpdateBalance(Arg.Any<Account>());
        }
    }
}
