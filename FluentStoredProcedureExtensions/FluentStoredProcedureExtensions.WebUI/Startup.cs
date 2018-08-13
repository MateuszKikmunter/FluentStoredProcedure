using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Infrastructure.Data;
using FluentStoredProcedureExtensions.Infrastructure.DataAccess;
using FluentStoredProcedureExtensions.Infrastructure.DataAccess.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluentStoredProcedureExtensions.WebUI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("FluentStoredProcedureConnection")));
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IStoredProcedureFactory, StoredProcedureFactory>();
            services.AddScoped<IStoredProcedure, StoredProcedure>();
            services.AddScoped<ISqlParameterFactory, SqlParameterFactory>();
            services.AddScoped<ICollectionToDataTableConverter, CollectionToDataTableConverter>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
