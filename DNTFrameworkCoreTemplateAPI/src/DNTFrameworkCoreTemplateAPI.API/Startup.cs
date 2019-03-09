using DNTFrameworkCore;
using DNTFrameworkCore.EntityFramework;
using DNTFrameworkCore.FluentValidation;
using DNTFrameworkCore.Web;
using DNTFrameworkCoreTemplateAPI.Application;
using DNTFrameworkCoreTemplateAPI.Application.Configuration;
using DNTFrameworkCoreTemplateAPI.API.Hubs;
using DNTFrameworkCoreTemplateAPI.Infrastructure;
using DNTFrameworkCoreTemplateAPI.Infrastructure.Context;
using DNTFrameworkCoreTemplateAPI.Resources;
using EFSecondLevelCache.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace DNTFrameworkCoreTemplateAPI.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ProjectSettings>(Configuration.Bind);
            services.AddDNTFramework()
                .AddDataAnnotationValidation()
                .AddModelValidation()
                .AddFluentModelValidation()
                .AddValidationOptions(options =>
                {
                    /*options.IgnoredTypes.Add(typeof());*/
                })
                .AddMemoryCache()
                .AddAuditingOptions(options =>
                {
                    // options.Enabled = true;
                    // options.EnabledForAnonymousUsers = false;
                    // options.IgnoredTypes.Add(typeof());
                    // options.Selectors.Add(new NamedTypeSelector("SelectorName", type => type == typeof()));
                }).AddTransactionOptions(options =>
                {
                    // options.Timeout=TimeSpan.FromMinutes(3);
                    //options.IsolationLevel=IsolationLevel.ReadCommitted;
                });

            services.AddDNTProtectionRepository<ProjectDbContext>();
            services.AddDNTCommonWeb()
                .AddDNTDataProtection();

            services.AddInfrastructure(Configuration);
            services.AddApplication();
            services.AddResources();
            services.AddWeb();
            services.AddJwtAuthentication(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("api", new Info
                {
                    Version = "api",
                    Title = "Services",
                    Description = "DNTFramework API Services",
                    Contact = new Contact
                    {
                        Email = "gholamrezarabbal@gmail.com",
                        Name = "GholamReza Rabbal",
                        Url = "http://www.dotnettips.info/user/%D8%BA%D9%84%D8%A7%D9%85%D8%B1%D8%B6%D8%A7%20%D8%B1%D8%A8%D8%A7%D9%84"
                    }
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    In = "header",
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                    {"oauth2", new string[] { }}
                });

                c.EnableAnnotations();

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Use(async (context, next) =>
                    {
                        var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                        if (error?.Error is SecurityTokenExpiredException)
                        {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                            {
                                Message = "authentication token expired"
                            }));
                        }
                        else if (error?.Error != null)
                        {
                            context.Response.StatusCode = 500;
                            context.Response.ContentType = "application/json";
                            const string message = "متأسفانه مشکلی در فرآیند انجام درخواست شما پیش آمده است!";

                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                            {
                                Message = message
                            }));
                        }
                        else
                        {
                            await next();
                        }
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDNTFramework();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseSignalR(routes => { routes.MapHub<NotificationHub>("/api/notificationhub"); });
            app.UseEFSecondLevelCache();

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Host = httpReq.Host.Value);
                c.RouteTemplate = "api-docs/{documentName}/swagger.json";

            });

            app.UseSwaggerUI(c =>
            {
                c.ShowExtensions();
                c.SwaggerEndpoint($"/api-docs/api/swagger.json", "api");
                c.DocExpansion(DocExpansion.None);
                c.DocumentTitle = "Services";
            });
        }
    }
}