namespace BankApi.Domain.Models.Response
{
    public class EventResonse
    {
        public bool IsValid { get; set; }
        public string ErroMessage { get; set; }

        public ResponseModel ResponseModel { get; set; }        
    }
}
