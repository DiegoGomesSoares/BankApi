namespace BankApi.Domain.Entities
{
    public class Account
    {
        public string Id { get; set; }
        public decimal Balance { get; set; }
        public Account()
        {
            Balance = 0;
        }
    }
}
