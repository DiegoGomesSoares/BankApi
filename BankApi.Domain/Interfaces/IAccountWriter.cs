using BankApi.Domain.Entities;

namespace BankApi.Domain.Interfaces
{
    public interface IAccountWriter
    {
        Task ResetAccountsAsync();
        Task CreateAccountAsyn(Account account);
        Task UpdateBalance(Account account);
    }
}
