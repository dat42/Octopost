namespace Octopost.Services.UoW
{
    using Octopost.Services.BusinessRules.Registry.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly Dictionary<DateTime, IUnitOfWork> createdUnitOfWorks = 
            new Dictionary<DateTime, IUnitOfWork>();

        private readonly IBusinessRuleRegistry businessRuleRegistry;

        private readonly string connectionString;

        public UnitOfWorkFactory(string connectionString, IBusinessRuleRegistry businessRuleRegistry)
        {
            this.connectionString = connectionString;
            this.businessRuleRegistry = businessRuleRegistry;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var unitOfWork = new UnitOfWork(this.connectionString, this.businessRuleRegistry);
            this.createdUnitOfWorks.Add(DateTime.Now, unitOfWork);
            return unitOfWork;
        }

        public IUnitOfWork GetLatest()
        {
            if (!this.createdUnitOfWorks.Any())
            {
                return this.CreateUnitOfWork();
            }

            var max = this.createdUnitOfWorks.Keys.Max();
            return this.createdUnitOfWorks[max];
        }
    }
}
