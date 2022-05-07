using BankApi.Domain.Interfaces;
using BankApi.Domain.Models.Requests;
using BankApi.Domain.Models.Response;

namespace BankApi.Processors
{
    public class TransferProcessExecutor : ITransferProcessExecutor
    {
        public IEventProcessor DepositProcessor { get; }
        public IEventProcessor WithDrawProcessor { get; }

        public TransferProcessExecutor(
            IEventProcessor withDrawProcessor,
            IEventProcessor depositProcessor)
        {
            DepositProcessor = depositProcessor ?? throw new ArgumentNullException(nameof(depositProcessor));
            WithDrawProcessor = withDrawProcessor ?? throw new ArgumentNullException(nameof(withDrawProcessor));
        }
        public async Task<EventResonse> Process(EventRequest request)
        {
            var withDrawResult = await WithDrawProcessor.Process(request);
            if (withDrawResult.IsValid == false) return withDrawResult;

            var depositResult = await DepositProcessor.Process(request);
            if (depositResult.IsValid == false) return depositResult;

            var response = CreateEventResponse(depositResult, withDrawResult);

            return response;
        }

        private static EventResonse CreateEventResponse(EventResonse depositResult, EventResonse withDrawResult)
        {
            return new EventResonse()
            {
                IsValid = true,
                ResponseModel = new ResponseModel()
                {
                    Origin = withDrawResult.ResponseModel.Origin,
                    Destination = depositResult.ResponseModel.Destination
                }
            };
        }
    }
}
