using BankApi.Domain.Entities;

namespace BankApi.Domain.Interfaces
{
    public  interface IAccountReader
    {
        Task<Account> GetByIdAsyn(string accountId);
    }
}
