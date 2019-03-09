using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

// https://github.com/rmaziarka/MediatR.Examples
namespace PurchaseOrderTracker.Web.Infrastructure
{
    public class HandleValidationExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
            {
                var apiException = context.Exception as ValidationException;
                // TODO:
                throw new NotImplementedException("handle validation exception");
                throw new Exception("not yet implemented");
// context.Result = new HttpResponseMessage(HttpStatusCode.BadRequest)
//                {
//                    Content = new StringContent(apiException.Message)
//                };
            }
        }
    }
}
