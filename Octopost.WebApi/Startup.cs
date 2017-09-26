namespace Octopost.WebApi
{
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Octopost.DataAccess.Context;
    using Octopost.Model.Data;
    using Octopost.Services.Posts;
    using Octopost.Services.Repository;
    using Octopost.Services.Tagging;
    using Octopost.Services.UoW;
    using Octopost.WebApi.Infrastructure.Filters;
    using System.Collections.Generic;
    using System.Reflection;
    using FluentValidation.AspNetCore;
    using Octopost.Services.Validation;
    using Octopost.WebApi.Infrastructure;
    using Octopost.WebApi.Infrastructure.Middleware;
    using Octopost.Services.Assembly;
    using System.Linq;
    using Octopost.Services;
    using Octopost.Services.ApiResult;
    using Octopost.Services.Votes;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            this.ConfigureAutomapper();
            var mvc = services.AddMvc(config =>
            {
                config.Filters.Add(typeof(ValidateActionFilter));
            });
            mvc.AddFluentValidation(fv =>
            {
                fv.ValidatorFactoryType = typeof(ValidationService);
                foreach (var assembly in AssemblyUtilities.GetOctopostAssemblies())
                {
                    fv.RegisterValidatorsFromAssembly(assembly);
                }
            });
            mvc.AddMvcOptions(o => o.Filters.Add(typeof(GlobalExceptionFilter)));
            services.AddSwaggerGen();
            services.AddEntityFrameworkSqlServer();
            var connectionString = this.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<OctopostDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddSingleton<IUnitOfWorkFactory>(x => new UnitOfWorkFactory(connectionString));
            services.AddSingleton(x =>
            {
                var factory = x.GetService<IUnitOfWorkFactory>();
                return factory.GetLatest();
            });
            services.AddScoped<IPostCreationService, PostCreationService>();
            services.AddScoped<IPostTaggingService, PostTaggingService>();
            services.AddScoped<IPostFilterService, PostFilterService>();
            services.AddScoped<IVoteCountService, VoteCountService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddSingleton<IAssemblyContainer>(x => new AssemblyContainer(this.GetAssemblies()));
            services.AddSingleton<IApiResultService, ApiResultService>();
            ServiceLocator.SetServiceLocator(() => services.BuildServiceProvider());

            var context = services.BuildServiceProvider().GetService<OctopostDbContext>();
            context.Database.EnsureCreated();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseMvc();
            app.UseMiddleware<NotFoundMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUi();
        }

        private void ConfigureAutomapper()
        {
            Mapper.Initialize(cfg => cfg.AddProfiles(this.GetAssemblies().ToList()));
            Mapper.AssertConfigurationIsValid();
        }

        private IEnumerable<Assembly> GetAssemblies() =>
            AssemblyUtilities.GetOctopostAssemblies();
    }
}
