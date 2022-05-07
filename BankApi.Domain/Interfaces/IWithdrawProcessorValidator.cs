using BankApi.Domain.Entities;
using BankApi.Domain.Models.Response;

namespace BankApi.Domain.Interfaces
{
    public interface IWithdrawProcessorValidator
    {
        EventResonse Validate(Account account, decimal amount);
    }
}
