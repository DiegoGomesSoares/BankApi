using AutoFixture.Idioms;
using BankApi.Creators;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Models.Requests;
using BankApi.UnitTests.AutoData;
using FluentAssertions;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace BankApi.UnitTests.Creators
{
    public class AccountCreatorTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardClauses(
            GuardClauseAssertion assertion)
            => assertion.Verify(typeof(AccountCreator).GetConstructors());

        [Theory, AutoNSubstituteData]
        public void Sut_ShouldImplementProperlyInterface(
            AccountCreator sut)
            => sut.Should().BeAssignableTo<IAccountCreator>();

        [Theory, AutoNSubstituteData]
        public async Task CreateAccount_ShouldCreateAccount(
            EventRequest request,
            AccountCreator sut)
        {
            sut.AccountWriter.CreateAccountAsyn(Arg.Do<Account>(x =>
            {
                x.Id.Should().Be(request.Destination);
                x.Balance.Should().Be(0);
            }));

            var actual = await sut.CreateAccount(request);

            actual.Id.Should().Be(request.Destination);

            await sut.AccountWriter.Received().CreateAccountAsyn(Arg.Any<Account>());
        }
    }
}
