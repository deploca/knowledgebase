using System;
using System.Collections.Generic;

namespace Knowledgebase.Application.Services
{
    public class _ServiceBase
    {
        private readonly IServiceProvider _serviceProvider;
        protected IServiceProvider ServiceProvider => _serviceProvider;

        private readonly UtilityServices.IAppSession _session;
        protected UtilityServices.IAppSession Session => _session;

        public _ServiceBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _session = this.GetService<UtilityServices.IAppSession>();
        }

        protected TService GetService<TService>()
        {
            return (TService)_serviceProvider.GetService(typeof(TService));
        }
    }
}
