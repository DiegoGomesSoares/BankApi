using BankApi.Domain.Enums;
using BankApi.Domain.Models.Requests;
using BankApi.Domain.Models.Response;

namespace BankApi.Domain.Interfaces
{
    public interface IEventProcessor
    {
        EventTypeEnum Type { get; }
        Task<EventResonse> Process(EventRequest request);
    }
}
