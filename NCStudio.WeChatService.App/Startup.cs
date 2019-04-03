using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NCStudio.WeChatService.Data;
using MediatR;
using NCStudio.Utility.Mvc.Behaviors;
using NCStudio.Utility.Mvc.Authentication;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Newtonsoft.Json;
using FluentValidation.AspNetCore;
using NCStudio.Utility.Mvc.Middleware;

namespace NCStudio.WeChatService.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private IContainer container;

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var host = Configuration["DBHOST"] ?? "localhost";
            var port = Configuration["DBPORT"] ?? "5432";
            var password = Configuration["DBPASSWORD"] ?? "nickchan9394";
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<WeChatServiceContext>(options =>
                {
                    options.UseNpgsql($"Server={host};Port={port};Database=WeChatServiceDb;User Id=postgres;Password={password}");
                });
            services.AddScoped<IWeChatServiceContext, WeChatServiceContext>();
            services.BuildServiceProvider().GetService<WeChatServiceContext>()
                .Database.EnsureCreated();

            services.AddMediatR(typeof(Startup));

            services.AddNCExceptionHandleBehavior();
            services.AddNCValidationBehavior();

            services
                .AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddJwtCookieAuthentication(Configuration["SECRET_KEY"]);

            var builder = new ContainerBuilder();

            builder.Populate(services);

            container = builder.Build();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsStaging())
            {
                app.UseResponseException();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors((builder) =>
            {
                builder.WithOrigins((Configuration["ALLOW_ORIGINS"] ?? "").Split(","));
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
                builder.AllowCredentials();
            });

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
