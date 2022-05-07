using BankApi.Domain.Enums;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers
{
    public class EventController : Controller
    {
        public IEventProcessorStrategy EventProcessorStrategy { get; }

        public EventController(IEventProcessorStrategy eventProcessorStrategy)
        {
            EventProcessorStrategy = eventProcessorStrategy ?? throw new ArgumentNullException(nameof(eventProcessorStrategy));
        }

        [HttpPost("event")]
        public async Task<IActionResult> Post([FromBody] EventRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var parsedType = (EventTypeEnum)Enum.Parse(typeof(EventTypeEnum), request.Type,true);

            var processor = EventProcessorStrategy.GetProcessor(parsedType);

            var response = await processor.Process(request);

            if (response.IsValid == false && string.IsNullOrEmpty(response.ErroMessage) == false) return BadRequest(response.ErroMessage);
            if (response.IsValid == false) return NotFound(0);           

            return Created("", response.ResponseModel);
        }
    }
}
