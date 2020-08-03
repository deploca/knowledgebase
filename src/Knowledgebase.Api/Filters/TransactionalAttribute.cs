using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Knowledgebase.UnitOfWork;

namespace Knowledgebase.Api.Filters
{
    public class TransactionalAttribute : Attribute, IAsyncActionFilter
    {
        public TransactionalAttribute()
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
            try
            {
                _unitOfWork.BeginTransaction();
                await next();

                // save data and commit transaction
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                throw ex;
            }
        }
    }
}
