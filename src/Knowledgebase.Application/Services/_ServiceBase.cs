using System;
using System.Collections.Generic;

namespace Knowledgebase.Application.Services
{
    public class _ServiceBase
    {
        private readonly IServiceProvider _serviceProvider;
        public IServiceProvider ServiceProvider => _serviceProvider;

        public _ServiceBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected TService GetService<TService>()
        {
            return (TService)_serviceProvider.GetService(typeof(TService));
        }
    }
}
