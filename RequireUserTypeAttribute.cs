using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace BillingService.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireUserTypeAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("usertype", out var userType) ||
                !string.Equals(userType.ToString(), "billing", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new UnauthorizedResult(); // Returns 401 Unauthorized
                return;
            }
            await next();
        }
    }
}