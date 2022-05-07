using BankApi.Domain.Enums;
using BankApi.Domain.Interfaces;

namespace BankApi.Processors
{
    public class EventProcessorStrategy : IEventProcessorStrategy
    {
        public IEnumerable<IEventProcessor> Processors { get; }

        public EventProcessorStrategy(IEnumerable<IEventProcessor> processors)
        {
            Processors = processors ?? throw new ArgumentNullException(nameof(processors));
        }

        public IEventProcessor GetProcessor(EventTypeEnum type)
        {
            var processor = Processors.FirstOrDefault(x => x.Type == type);

            if (processor == null)
                throw new NotImplementedException();

            return processor;
        }
    }
}
