using BankApi.Domain.Enums;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Models.Requests;
using BankApi.Domain.Models.Response;

namespace BankApi.Processors
{
    public class TransferProcessor : IEventProcessor
    {
        public EventTypeEnum Type => EventTypeEnum.Transfer;
        public ITransferProcessExecutor TransferProcssor { get; }

        public TransferProcessor(
            ITransferProcessExecutor transferProcssor)
        {
            TransferProcssor = transferProcssor ?? throw new ArgumentNullException(nameof(transferProcssor));
        }

        public async Task<EventResonse> Process(EventRequest request)
        {
            return await TransferProcssor.Process(request);
        }
    }
}
