using BankApi.Domain.Models.Requests;
using BankApi.Domain.Models.Response;

namespace BankApi.Domain.Interfaces
{
    public  interface ITransferProcessExecutor
    {
        Task<EventResonse> Process(EventRequest request);
    }
}
