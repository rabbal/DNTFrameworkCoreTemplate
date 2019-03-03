using System;
using CacheManager.Core;
using DNTFrameworkCore;
using DNTFrameworkCore.Dependency;
using DNTFrameworkCore.EntityFramework;
using DNTFrameworkCore.Eventing;
using DNTFrameworkCore.Localization;
using DNTFrameworkCoreTemplateAPI.Application.Identity;
using DNTFrameworkCoreTemplateAPI.Infrastructure.Context;
using DNTFrameworkCoreTemplateAPI.Resources;
using EFSecondLevelCache.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace DNTFrameworkCoreTemplateAPI.UnitTests
{
     public enum DatabaseEngine
    {
        InMemory,
        SQLite
    }

    public static class TestingHelper
    {
        public static IServiceProvider BuildServiceProvider(Action<IServiceCollection> configure = null,
            DatabaseEngine database = DatabaseEngine.InMemory, SqliteConnection connection = null)
        {
            var services = new ServiceCollection();

            services.Scan(scan => scan
                .FromAssemblyOf<IUserService>()
                .AddClasses(classes => classes.AssignableTo<ISingletonDependency>())
                .AsMatchingInterface()
                .WithSingletonLifetime()
                .AddClasses(classes => classes.AssignableTo<IScopedDependency>())
                .AsMatchingInterface()
                .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo<ITransientDependency>())
                .AsMatchingInterface()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IBusinessEventHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            services.AddLogging();
            services.AddLocalization();
            services.AddNullLocalization();
            services.AddResources();
            services.AddDNTFramework();
            services.AddDNTUnitOfWork<ProjectDbContext>();
            services.AddEFSecondLevelCache();
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
            services.AddSingleton(typeof(ICacheManagerConfiguration),
                new ConfigurationBuilder()
                    .WithJsonSerializer()
                    .WithMicrosoftMemoryCacheHandle()
                    .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(10))
                    .Build());

            switch (database)
            {
                case DatabaseEngine.SQLite:
                    services.AddEntityFrameworkSqlite()
                        .AddDbContext<ProjectDbContext>(builder =>
                            builder.UseSqlite(connection ?? throw new ArgumentNullException(nameof(connection)))
                                .ConfigureWarnings(warnings =>
                                {
                                    warnings.Throw(RelationalEventId.QueryClientEvaluationWarning);
                                    warnings.Throw(CoreEventId.IncludeIgnoredWarning);
                                }));
                    break;
                case DatabaseEngine.InMemory:
                    services.AddEntityFrameworkInMemoryDatabase()
                        .AddDbContext<ProjectDbContext>(builder => builder.UseInMemoryDatabase("SharedDatabaseName"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(database), database, null);
            }


            configure?.Invoke(services);

            var provider = IoC.ApplicationServices = services.BuildServiceProvider();

            provider.RunScoped<ProjectDbContext>(context =>
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            });

            return provider;
        }
    }
}