using Microsoft.AspNetCore.Mvc.Filters;

namespace BankApi.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return base.OnExceptionAsync(context);
        }
    }
}
