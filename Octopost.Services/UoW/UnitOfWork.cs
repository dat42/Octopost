namespace Octopost.Services.UoW
{
    using Microsoft.EntityFrameworkCore;
    using Octopost.DataAccess.Context;
    using Octopost.Model.Data;
    using Octopost.Services.Repository;
    using System;
    using System.Collections.Generic;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly OctopostDbContext octopostDbContext;

        private readonly string connectionString;

        private readonly IDictionary<Type, Type> repositories = new Dictionary<Type, Type>();

        public UnitOfWork(string connectionString)
        {
            this.RegisterRepositories();
            this.connectionString = connectionString;
            this.octopostDbContext = this.CreateContext();
        }

        public IRepository<T> CreateEntityRepository<T>()
        {
            if (this.repositories.ContainsKey(typeof(T)))
            {
                throw new ArgumentException($"No repository for type {typeof(T).FullName} registered");
            }

            var type = this.repositories[typeof(T)];
            return (IRepository<T>)Activator.CreateInstance(type, this.octopostDbContext);
        }

        public void Dispose() =>
            this.octopostDbContext.Dispose();

        public void Save() =>
            this.octopostDbContext.SaveChanges();

        private OctopostDbContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<OctopostDbContext>();
            optionsBuilder.UseSqlServer(this.connectionString);
            return new OctopostDbContext(optionsBuilder.Options);
        }

        private void RegisterRepositories()
        {
            this.repositories.Add(typeof(Post), typeof(Repository<Post>));
            this.repositories.Add(typeof(Vote), typeof(Repository<Vote>));
        }
    }
}
