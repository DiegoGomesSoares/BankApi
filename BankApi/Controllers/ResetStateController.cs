using BankApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BankApi.Controllers
{
    public class ResetStateController : Controller
    {
        public IAccountWriter AccountWriter { get; }
        public ResetStateController(IAccountWriter accountWriter)
        {
            AccountWriter = accountWriter ?? throw new ArgumentNullException(nameof(accountWriter));
        }

        [HttpPost("reset")]
        public async Task<IActionResult> Reset()
        {
            await AccountWriter.ResetAccountsAsync();

            return Ok(HttpStatusCode.OK.ToString());            
        }
    }
}
