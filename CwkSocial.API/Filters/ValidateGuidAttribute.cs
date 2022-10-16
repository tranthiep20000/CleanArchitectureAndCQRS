using CwkSocial.API.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CwkSocial.API.Filters
{
    public class ValidateGuidAttribute : ActionFilterAttribute
    {
        private readonly string _key;

        public ValidateGuidAttribute(string key)
        {
            _key = key;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.TryGetValue(_key, out var value)) return;

            if (Guid.TryParse(value?.ToString(), out var guid)) return;

            var apiError = new ErrorResponse();
            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad Requet";
            apiError.TimeStamp = DateTime.UtcNow;
            apiError.Errors.Add($" The identity for {guid} is not correct GUID format");

            context.Result = new JsonResult(apiError);
        }
    }
}