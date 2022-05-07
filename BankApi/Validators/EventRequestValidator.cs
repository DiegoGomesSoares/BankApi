using BankApi.Domain.Enums;
using BankApi.Domain.Models.Requests;
using FluentValidation;

namespace BankApi.Validators
{
    public class EventRequestValidator : AbstractValidator<EventRequest>
    {
        public EventRequestValidator()
        {
            RuleFor(x => x.Type)
                .Must(BeAValidEnum);

            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Destination)
               .NotEmpty()
               .When(x => IsDepositOrTransferType(x.Type), ApplyConditionTo.CurrentValidator);

            RuleFor(x => x.Origin)
               .NotEmpty()
               .When(x => IsWithdrawOrTransferType(x.Type), ApplyConditionTo.CurrentValidator);
        }

        private bool IsWithdrawOrTransferType(string type)
        {
            Enum.TryParse(typeof(EventTypeEnum), type, true, out var convertedType);

            return (EventTypeEnum)convertedType == EventTypeEnum.Withdraw
                    || (EventTypeEnum)convertedType == EventTypeEnum.Transfer;
        }

        private static bool IsDepositOrTransferType(string type)
        {
            Enum.TryParse(typeof(EventTypeEnum), type, true, out var convertedType);

            return (EventTypeEnum)convertedType == EventTypeEnum.Deposit
                    || (EventTypeEnum)convertedType == EventTypeEnum.Transfer;
        }

        private bool BeAValidEnum(string type)
        {
            var isValid = Enum.TryParse(typeof(EventTypeEnum), type, true, out _);
            return isValid;
        }
    }
}
