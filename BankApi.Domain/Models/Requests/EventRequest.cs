using BankApi.Domain.Enums;

namespace BankApi.Domain.Models.Requests
{
    public class EventRequest
    {
        public string Type { get; set; }
        public string Destination { get; set; }
        public decimal Amount { get; set; }
        public string Origin { get; set; }
    }
}
