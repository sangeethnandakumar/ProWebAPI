using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OData.Swagger.Services;
using ProWeb.Service.Implementations;
using ProWeb.Service.Interfaces;
using ProWebAPI.Extensions;
using ProWebAPI.Filters;
using System.Linq;
using Microsoft.AspNetCore.OData;

namespace ProWebAPI
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
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            })
            .AddNewtonsoftJson(options =>
            {
                options.UseMemberCasing();
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProWebAPI", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddControllers().AddOData(x=>x.Select().Count().Filter().OrderBy().SetMaxTop(100).Expand());
            services.AddOdataSwaggerSupport();

            //DI Components
            services.AddSingleton<IGoalService, GoalService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProWebAPI v1"));
            }
            app.UseRouting();
            app.UseAuthorization();
            app.UseGlobalExceptionHandler();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}