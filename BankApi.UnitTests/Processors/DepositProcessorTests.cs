using AutoFixture.Idioms;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Models.Requests;
using BankApi.Processors;
using BankApi.UnitTests.AutoData;
using FluentAssertions;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace BankApi.UnitTests.Processors
{
    public class DepositProcessorTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardClauses(
            GuardClauseAssertion assertion)
            => assertion.Verify(typeof(DepositProcessor).GetConstructors());

        [Theory, AutoNSubstituteData]
        public void Sut_ShouldImplementProperlyInterface(
            DepositProcessor sut)
            => sut.Should().BeAssignableTo<IEventProcessor>();

        [Theory, AutoNSubstituteData]
        public async Task Process_WhenAccountWasNotFounded_ShouldCreateAccountAndProcessCorrectly(
            EventRequest request,
            Account account,
            DepositProcessor sut)
        {
            sut.AccountReader.GetByIdAsyn(Arg.Any<string>()).Returns((Account)null);
            sut.AccountCreator.CreateAccount(Arg.Any<EventRequest>()).Returns(account);

            var actual = await sut.Process(request);

            actual.IsValid.Should().BeTrue();
            actual.ResponseModel.Destination.Id.Should().Be(account.Id);
            actual.ResponseModel.Destination.Balance.Should().Be(account.Balance);

            await sut.AccountReader.Received().GetByIdAsyn(request.Destination);
            await sut.AccountCreator.Received().CreateAccount(request);
            await sut.AccountCashin.Received().Cashin(account, request.Amount);
        }
    }
}
