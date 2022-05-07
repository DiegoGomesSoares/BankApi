using BankApi.Domain.Enums;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Models.Requests;
using BankApi.Domain.Models.Response;

namespace BankApi.Processors
{
    public class WithdrawProcessor : IEventProcessor
    {
        public EventTypeEnum Type => EventTypeEnum.Withdraw;
        public IWithdrawProcessorValidator WithdrawProcessorValidator { get; }
        public IAccountReader AccountReader { get; }

        public IAccountCashout AccountCashout { get;  }

        public WithdrawProcessor(
            IWithdrawProcessorValidator withdrawProcessorValidator,
            IAccountCashout accountCashout,
            IAccountReader accountReader)
        {
            WithdrawProcessorValidator = withdrawProcessorValidator ?? throw new ArgumentNullException(nameof(withdrawProcessorValidator));
            AccountCashout = accountCashout ?? throw new ArgumentNullException(nameof(accountCashout));
            AccountReader = accountReader ?? throw new ArgumentNullException(nameof(accountReader));
        }

        public async Task<EventResonse> Process(EventRequest request)
        {
            var account = await AccountReader.GetByIdAsyn(request.Origin);
            if (account == null) return CreateNotFoundResponse();
            
            var validationResult = WithdrawProcessorValidator.Validate(account, request.Amount);
            if (validationResult.IsValid == false) return validationResult;

            await AccountCashout.Cashout(account, request.Amount);
            var response = CreateEventResponse(account);

            return response;
        }

        private static EventResonse CreateEventResponse(Domain.Entities.Account account)
        {
            return new EventResonse()
            {
                IsValid = true,
                ResponseModel = new ResponseModel()
                {
                    Origin = new OriginResponse()
                    {
                        Id = account.Id,
                        Balance = account.Balance
                    }
                }
            };
        }

        private static EventResonse CreateNotFoundResponse()
        {
            return new EventResonse { IsValid = false };
        }
    }
}
