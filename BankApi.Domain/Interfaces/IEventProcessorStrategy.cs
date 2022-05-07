using BankApi.Domain.Enums;

namespace BankApi.Domain.Interfaces
{
    public interface IEventProcessorStrategy
    {
        public IEventProcessor GetProcessor(EventTypeEnum type);
    }
}
