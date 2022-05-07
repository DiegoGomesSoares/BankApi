using BankApi.Domain.Entities;
using BankApi.Domain.Models.Requests;

namespace BankApi.Domain.Interfaces
{
    public interface IAccountCreator
    {
        Task<Account> CreateAccount(EventRequest request);
    }
}
