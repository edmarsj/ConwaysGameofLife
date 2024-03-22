using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ConwaysGameofLife.Core.Exceptions;
using ConwaysGameofLife.API.Models;

namespace ConwaysGameofLife.API.Filters
{
    public class ExceptionFilter : IActionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                return;
            }

            var response = new ResponseModel();
            var result = new JsonResult(response);

            if (context.Exception is KnownException knownException)
            {
                response.ErrorCode = knownException.Message;
                result.StatusCode = (int)knownException.StatusCode;
            }
            else
            {
                response.ErrorCode = "internal_server_error";
                result.StatusCode = 500;
                _logger.LogError(context.Exception,
                                 message: context.Exception.Message);
            }

            context.Result = result;
            context.ExceptionHandled = true;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var result = new JsonResult(new ValidationResponseModel(context.ModelState))
                {
                    StatusCode = 400
                };
                context.Result = result;
            }
        }
    }
}
