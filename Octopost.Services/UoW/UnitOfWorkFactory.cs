namespace Octopost.Services.UoW
{
    using Octopost.Services.BusinessRules.Registry.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly Dictionary<int, IUnitOfWork> createdUnitOfWorks = 
            new Dictionary<int, IUnitOfWork>();

        private readonly IBusinessRuleRegistry businessRuleRegistry;

        private readonly string connectionString;

        private static readonly object lockObj = new object();

        public UnitOfWorkFactory(string connectionString, IBusinessRuleRegistry businessRuleRegistry)
        {
            this.connectionString = connectionString;
            this.businessRuleRegistry = businessRuleRegistry;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var unitOfWork = new UnitOfWork(this.connectionString, this.businessRuleRegistry);
            lock (UnitOfWorkFactory.lockObj)
            {
                this.createdUnitOfWorks.Add(
                    this.createdUnitOfWorks.Any()
                        ? this.createdUnitOfWorks.Max(x => x.Key) + 1
                        : 0,
                    unitOfWork);
            }

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
