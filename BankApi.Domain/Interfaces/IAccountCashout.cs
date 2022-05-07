using BankApi.Domain.Entities;

namespace BankApi.Domain.Interfaces
{
    public interface IAccountCashout
    {
        Task Cashout(Account account, decimal amount);
    }
}
