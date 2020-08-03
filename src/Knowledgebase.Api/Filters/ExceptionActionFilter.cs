using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Knowledgebase.Api.Filters
{
    public class ExceptionActionFilter : ExceptionFilterAttribute
    {
        public ExceptionActionFilter()
        {
        }

        public override void OnException(ExceptionContext context)
        {
            if (isTypeOf(context.Exception, typeof(Exceptions.AppException)))
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode =
                    isTypeOf(context.Exception, typeof(Exceptions.BadRequestException)) ? (int)HttpStatusCode.BadRequest :
                    isTypeOf(context.Exception, typeof(Exceptions.ForbiddenRequestException)) ? (int)HttpStatusCode.Forbidden :
                    isTypeOf(context.Exception, typeof(Exceptions.UnauthorizedException)) ? (int)HttpStatusCode.Unauthorized :
                    (int)HttpStatusCode.InternalServerError;

                context.Result = new JsonResult(new
                {
                    Message = context.Exception.Message,
                    InnerMessage = context.Exception.InnerException != null ? context.Exception.InnerException.Message : null,
                    Source = context.Exception.Source,
                });
            }
            else
            {
                base.OnException(context);
            }
        }

        private bool isTypeOf(Exception exception, Type baseType)
        {
            return exception.GetType() == baseType || exception.GetType().IsSubclassOf(baseType);
        }
    }
}
