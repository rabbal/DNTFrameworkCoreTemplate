using System;
using System.IO;
using System.Reflection;
using CacheManager.Core;
using DNTFrameworkCore;
using DNTFrameworkCore.Dependency;
using DNTFrameworkCore.EFCore;
using DNTFrameworkCore.Eventing;
using DNTFrameworkCore.FluentValidation;
using DNTFrameworkCore.Localization;
using DNTFrameworkCore.Web;
using DNTFrameworkCoreTemplateAPI.Application;
using DNTFrameworkCoreTemplateAPI.Infrastructure.Context;
using DNTFrameworkCoreTemplateAPI.IntegrationTests.Stubs;
using DNTFrameworkCoreTemplateAPI.Resources;
using EFSecondLevelCache.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DNTFrameworkCoreTemplateAPI.IntegrationTests
{
    public enum DatabaseEngine
    {
        LocalDb,
        SqlServerExpress,
        SQLite,
    }

    public static class TestingHelper
    {
        public static IServiceProvider BuildServiceProvider(DatabaseEngine database, SqliteConnection connection = null,
            Action<IServiceCollection> configure = null)
        {
            var services = new ServiceCollection();

            services.AddApplication();

            services.AddLogging();
            services.AddLocalization();
            services.AddNullLocalization();
            services.AddResources();
            services.AddEFCore<ProjectDbContext>();
            services.AddEFSecondLevelCache();
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
            services.AddSingleton(typeof(ICacheManagerConfiguration),
                new CacheManager.Core.ConfigurationBuilder()
                    .WithJsonSerializer()
                    .WithMicrosoftMemoryCacheHandle()
                    .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(10))
                    .Build());

            services.AddDNTFrameworkCore()
                .AddModelValidation()
                .AddFluentModelValidation();
            services.AddMemoryCache();

            //For IPasswordHasher Implementation
            services.AddDNTCommonWeb();
            
            var fileName =
                Path.Combine(
                    Path.GetDirectoryName(
                        typeof(TestingHelper).GetTypeInfo().Assembly.Location),
                    "ProjectIntegrationTesting.mdf");

            switch (database)
            {
                case DatabaseEngine.LocalDb:
                    services.AddEntityFrameworkSqlServer()
                        .AddDbContext<ProjectDbContext>(builder =>
                            builder.UseSqlServer(
                                    $@"Data Source=(LocalDB)\MSSQLLocalDb;Initial Catalog=ProjectIntegrationTesting;Integrated Security=True;
                                    MultipleActiveResultSets=true;AttachDbFileName={fileName}")
                                .ConfigureWarnings(warnings =>
                                {
                                    warnings.Throw(RelationalEventId.QueryClientEvaluationWarning);
                                    warnings.Throw(CoreEventId.IncludeIgnoredWarning);
                                }));
                    break;
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
                case DatabaseEngine.SqlServerExpress:
                    services.AddEntityFrameworkSqlServer()
                        .AddDbContext<ProjectDbContext>(builder =>
                            builder.UseSqlServer(
                                    $@"Data Source=.\SQLEXPRESS;Initial Catalog=ProjectIntegrationTesting;Integrated Security=True;
                                    MultipleActiveResultSets=true;AttachDbFileName={fileName};User Instance=True")
                                .ConfigureWarnings(warnings =>
                                {
                                    warnings.Throw(RelationalEventId.QueryClientEvaluationWarning);
                                    warnings.Throw(CoreEventId.IncludeIgnoredWarning);
                                }));


                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(database), database, null);
            }

            services.Replace(ServiceDescriptor.Singleton<IEventBus, StubEventBus>());

            configure?.Invoke(services);

            var serviceProvider = IoC.ApplicationServices = services.BuildServiceProvider();

            serviceProvider.RunScoped<ProjectDbContext>(context =>
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            });

            return serviceProvider;
        }
    }
}