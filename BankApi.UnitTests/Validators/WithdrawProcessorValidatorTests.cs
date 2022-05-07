using AutoFixture.Idioms;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.UnitTests.AutoData;
using BankApi.Validators;
using FluentAssertions;
using Xunit;

namespace BankApi.UnitTests.Validators
{
    public class WithdrawProcessorValidatorTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardClauses(
            GuardClauseAssertion assertion)
            => assertion.Verify(typeof(WithdrawProcessorValidator).GetConstructors());

        [Theory, AutoNSubstituteData]
        public void Sut_ShouldImplementProperlyInterface(
            WithdrawProcessorValidator sut)
            => sut.Should().BeAssignableTo<IWithdrawProcessorValidator>();

        [Theory, AutoNSubstituteData]
        public void ValidateAsync_WhenAccountBalanceIsLessThenAmount_ReturnBadRequestResponse(
            Account account,
            WithdrawProcessorValidator sut)
        {
            account.Balance = 50;
            decimal amount = 500;

            var actual = sut.Validate(account, amount);

            actual.IsValid.Should().BeFalse();
            actual.ErroMessage.Should().Be("Invalid balance.");
        }
    }
}
