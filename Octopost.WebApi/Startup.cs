namespace Octopost.WebApi
{
    using AutoMapper;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Octopost.DataAccess.Context;
    using Octopost.Services;
    using Octopost.Services.ApiResult;
    using Octopost.Services.Assembly;
    using Octopost.Services.BusinessRules.Interfaces;
    using Octopost.Services.BusinessRules.Registry;
    using Octopost.Services.BusinessRules.Registry.Interfaces;
    using Octopost.Services.Posts;
    using Octopost.Services.Tagging;
    using Octopost.Services.UoW;
    using Octopost.Services.Validation;
    using Octopost.Services.Votes;
    using Octopost.Validation.Common;
    using Octopost.Validation.Dto.Newsletter;
    using Octopost.WebApi.Infrastructure;
    using Octopost.WebApi.Infrastructure.Filters;
    using Octopost.WebApi.Infrastructure.Middleware;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // This instance needs to be created for the compiler to reference the Octopost.Validation assembly
            var instance = new PostDtoValidator();

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
            services.AddTransient<IValidatorFactory, ValidationService>();
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

            services.AddSingleton<IUnitOfWorkFactory>(x =>
            {
                var businessRuleRegistry = x.GetService<IBusinessRuleRegistry>();
                var result = new UnitOfWorkFactory(connectionString, businessRuleRegistry);
                return result;
            });
            services.AddSingleton(x =>
            {
                var factory = x.GetService<IUnitOfWorkFactory>();
                return factory.GetLatest();
            });
            this.ConfigureBusinessRules(services);
            services.AddSingleton<IBusinessRuleRegistry, BaseBusinessRuleRegistry>();
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

        private void ConfigureBusinessRules(IServiceCollection services)
        {
            var octopostAssemblies = this.GetAssemblies();
            foreach (var octopostAssembly in octopostAssemblies)
            {
                var types = octopostAssembly.GetTypes();
                var businessRules = types.Where(x => x.GetInterfaces().Contains(typeof(IBusinessRuleBase)) && !x.IsInterface);
                foreach (var businessRule in businessRules)
                {
                    if (!businessRule.IsAbstract && !businessRule.IsGenericType)
                    {
                        services.AddTransient(businessRule);
                    }
                }
            }
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
