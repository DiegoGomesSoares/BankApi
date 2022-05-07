using AutoFixture.Idioms;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Models.Requests;
using BankApi.Domain.Models.Response;
using BankApi.Processors;
using BankApi.UnitTests.AutoData;
using FluentAssertions;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace BankApi.UnitTests.Processors
{
    public class WithdrawProcessorTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardClauses(
            GuardClauseAssertion assertion)
            => assertion.Verify(typeof(WithdrawProcessor).GetConstructors());

        [Theory, AutoNSubstituteData]
        public void Sut_ShouldImplementProperlyInterface(
            WithdrawProcessor sut)
            => sut.Should().BeAssignableTo<IEventProcessor>();

        [Theory, AutoNSubstituteData]
        public async Task Process_WhenAccountWasNotFounded_ReturnNotFoundResult(
            EventRequest request,
            WithdrawProcessor sut)
        {
            sut.AccountReader.GetByIdAsyn(Arg.Any<string>()).Returns((Account)null);

            var actual = await sut.Process(request);

            actual.IsValid.Should().BeFalse();

            await sut.AccountReader.Received().GetByIdAsyn(request.Origin);
            sut.WithdrawProcessorValidator.DidNotReceive().Validate(Arg.Any<Account>(), Arg.Any<decimal>());
            await sut.AccountCashout.DidNotReceive().Cashout(Arg.Any<Account>(), Arg.Any<decimal>());
        }

        [Theory, AutoNSubstituteData]
        public async Task Process_WhenWithdrawProcessorValidatorReturnInvalid_ReturnvalidationResult(
            EventRequest request,
            Account account,
            EventResonse validatorResponse,
            WithdrawProcessor sut)
        {
            sut.AccountReader.GetByIdAsyn(Arg.Any<string>()).Returns(account);

            validatorResponse.IsValid = false;
            sut.WithdrawProcessorValidator
                .Validate(Arg.Any<Account>(), Arg.Any<decimal>())
                    .Returns(validatorResponse);

            var actual = await sut.Process(request);

            actual.Should().Be(validatorResponse);

            await sut.AccountReader.Received().GetByIdAsyn(request.Origin);
            sut.WithdrawProcessorValidator.Received().Validate(account, request.Amount);
            await sut.AccountCashout.DidNotReceive().Cashout(Arg.Any<Account>(), Arg.Any<decimal>());
        }

        [Theory, AutoNSubstituteData]
        public async Task Process_ReturnResult(
            EventRequest request,
            Account account,
            EventResonse validatorResponse,
            WithdrawProcessor sut)
        {
            sut.AccountReader.GetByIdAsyn(Arg.Any<string>()).Returns(account);

            validatorResponse.IsValid = true;
            sut.WithdrawProcessorValidator
                .Validate(Arg.Any<Account>(), Arg.Any<decimal>())
                    .Returns(validatorResponse);

            var actual = await sut.Process(request);

            actual.IsValid.Should().BeTrue();
            actual.ResponseModel.Origin.Id.Should().Be(account.Id);
            actual.ResponseModel.Origin.Balance.Should().Be(account.Balance);

            await sut.AccountReader.Received().GetByIdAsyn(request.Origin);
            sut.WithdrawProcessorValidator.Received().Validate(account, request.Amount);
            await sut.AccountCashout.Received().Cashout(account, request.Amount);
        }
    }
}
