using AutoFixture.Idioms;
using BankApi.Domain.Enums;
using BankApi.Domain.Models.Requests;
using BankApi.UnitTests.AutoData;
using BankApi.Validators;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace BankApi.UnitTests.Validators
{
    public  class EventRequestValidatorTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardClauses(GuardClauseAssertion assertion)
            => assertion.Verify(typeof(EventRequestValidator).GetConstructors());

        [Theory, AutoNSubstituteData]
        public void Sut_ShouldImplementProperlyInterface(
            EventRequestValidator sut)
            => sut.Should().BeAssignableTo<AbstractValidator<EventRequest>>();


        [Theory]
        [AutoInlineData(EventTypeEnum.Deposit)]
        [AutoInlineData(EventTypeEnum.Withdraw)]
        [AutoInlineData(EventTypeEnum.Transfer)]
        public void EventRequest_ShouldBeValid(
            EventTypeEnum type,
            EventRequest request)
        {
            request.Type = type.ToString();
            var result = new EventRequestValidator()
                .TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Type);
            result.ShouldNotHaveValidationErrorFor(x => x.Amount);
            result.ShouldNotHaveValidationErrorFor(x => x.Destination);
            result.ShouldNotHaveValidationErrorFor(x => x.Origin);
        }

        [Theory, AutoNSubstituteData]        
        public void EventRequest_WhenAmountInvalid_ShouldReturnError(            
            EventRequest request)
        {
            request.Type = EventTypeEnum.Deposit.ToString();
            request.Amount = -1;
            var result = new EventRequestValidator()
                .TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Amount);           
        }

        [Theory, AutoNSubstituteData]
        public void EventRequest_WhenDestinationInvalid_ShouldReturnError(
            EventRequest request)
        {
            request.Type = EventTypeEnum.Deposit.ToString();
            request.Amount = 50;
            request.Destination = "";
            var result = new EventRequestValidator()
                .TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Destination);
        }

        [Theory, AutoNSubstituteData]
        public void EventRequest_WhenOriginInvalid_ShouldReturnError(
            EventRequest request)
        {
            request.Type = EventTypeEnum.Withdraw.ToString();
            request.Amount = 50;
            request.Origin = "";
            var result = new EventRequestValidator()
                .TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Origin);
        }
    }
}
