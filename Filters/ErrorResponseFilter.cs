using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Stock_Back_End.Models.ErrorModels;
namespace Stock_Back_End.Filters
{
    public class ErrorResponseFilter : IExceptionFilter
    {
        /*
         For any non handled exception this will be executed.
         */
        public void OnException(ExceptionContext context)
        {
            var errorResponse = ErrorResponse.From(context.Exception);
            context.Result = new ObjectResult(errorResponse) { StatusCode = 500 };
        }

    }
}
