using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Models.Response;

namespace BankApi.Validators
{
    public class WithdrawProcessorValidator : IWithdrawProcessorValidator
    {
        public EventResonse Validate(Account account, decimal amount)
        {
            if (account.Balance < amount)
            {
                return CreateBadRequestResponse();
            }

            return new EventResonse { IsValid = true };
        }

        private EventResonse CreateBadRequestResponse()
        {
            return new EventResonse { IsValid = false, ErroMessage = "Invalid balance." };
        }
    }
}
