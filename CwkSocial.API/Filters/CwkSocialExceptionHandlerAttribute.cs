using CwkSocial.API.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CwkSocial.API.Filters
{
    public class CwkSocialExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var apiError = new ErrorResponse();
            apiError.StatusCode = 500;
            apiError.StatusPhrase = "Internal Server Error";
            apiError.TimeStamp = DateTime.UtcNow;
            apiError.Errors.Add(context.Exception.Message);

            context.Result = new JsonResult(apiError);
        }
    }
}
