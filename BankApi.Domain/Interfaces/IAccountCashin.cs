using BankApi.Domain.Entities;

namespace BankApi.Domain.Interfaces
{
    public interface IAccountCashin
    {
        Task Cashin(Account account, decimal amount);
    }
}
