using BankApi.Domain.Enums;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Models.Requests;
using BankApi.Domain.Models.Response;

namespace BankApi.Processors
{
    public class DepositProcessor : IEventProcessor
    {
        public EventTypeEnum Type => EventTypeEnum.Deposit;

        public IAccountReader AccountReader { get; }
        public IAccountCreator AccountCreator { get; }
        public IAccountCashin AccountCashin { get; set; }

        public DepositProcessor(
            IAccountReader accountReader,
            IAccountCreator accountCreator,
            IAccountCashin accountCashin)
        {
            AccountReader = accountReader ?? throw new ArgumentNullException(nameof(accountReader));
            AccountCreator = accountCreator ?? throw new ArgumentNullException(nameof(accountCreator));
            AccountCashin = accountCashin ?? throw new ArgumentNullException(nameof(accountCashin));
        }

        public async Task<EventResonse> Process(EventRequest request)
        {
            var account = await AccountReader.GetByIdAsyn(request.Destination);
            if (account == null) account = await AccountCreator.CreateAccount(request);

            await AccountCashin.Cashin(account, request.Amount);

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
                    Destination = new DestinationResponse()
                    {
                        Id = account.Id,
                        Balance = account.Balance
                    }
                }
            };
        }
    }
}
