using BankApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers
{
    public class BalanceController : Controller
    {
        public IAccountReader AccountReader { get; }
        public BalanceController(IAccountReader accountReader)
        {
            AccountReader = accountReader ?? throw new ArgumentNullException(nameof(accountReader));
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalanceByAccountIdAsync([FromQuery] string account_id)
        {
            var account = await AccountReader.GetByIdAsyn(account_id);

            if (account == null) return NotFound(0);

            return Ok(account.Balance);
        }
    }
}
