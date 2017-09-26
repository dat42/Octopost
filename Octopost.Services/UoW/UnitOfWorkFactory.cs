namespace Octopost.Services.UoW
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly Dictionary<DateTime, IUnitOfWork> createdUnitOfWorks = 
            new Dictionary<DateTime, IUnitOfWork>();

        private readonly string connectionString;

        public UnitOfWorkFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var unitOfWork = new UnitOfWork(this.connectionString);
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
