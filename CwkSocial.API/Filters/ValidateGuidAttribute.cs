using CwkSocial.API.Contracts.Common;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CwkSocial.API.Filters
{
    public class ValidateGuidAttribute : ActionFilterAttribute
    {
        private readonly List<string> _keys;

        public ValidateGuidAttribute(string key)
        {
            _keys = new List<string>();
            _keys.Add(key);
        }

        public ValidateGuidAttribute(string key1, string key2)
        {
            _keys = new List<string>();
            _keys.Add(key1);
            _keys.Add(key2);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isCheck = false;
            var apiError = new ErrorResponse();

            foreach (var key in _keys)
            {
                if (!context.ActionArguments.TryGetValue(key, out var value)) return;

                if (Guid.TryParse(value?.ToString(), out var guid))
                {
                    isCheck = true;
                    apiError.Errors.Add($" The identity for {key} is not correct GUID format");
                };
            }

            if (isCheck)
            {
                apiError.StatusCode = 400;
                apiError.StatusPhrase = "Bad Requet";
                apiError.TimeStamp = DateTime.UtcNow;
            }
        }
    }
}