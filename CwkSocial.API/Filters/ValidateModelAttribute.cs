using CwkSocial.API.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CwkSocial.API.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ModelState.IsValid)
            {
                var apiError = new ErrorResponse();
                apiError.StatusCode = 400;
                apiError.StatusPhrase = "Bad Request";
                apiError.TimeStamp = DateTime.UtcNow;

                var errors = context.ModelState.AsEnumerable();

                foreach (var error in errors)
                {
                    apiError.Errors.Add(error.Value.ToString());
                }

                context.Result = new JsonResult(apiError) { StatusCode = 400 };
                // TO DO: Make sure Asp .Net Core doesn't override our action result body
            }    
        }
    }
}
