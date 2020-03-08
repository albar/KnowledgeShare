using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunnypro.SpaService.VueCli;
using KnowledgeShare.Store.Core;
using KnowledgeShare.Store.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KnowledgeShare.Server
{
    public class Startup
    {
        private static readonly string DevelopmentOnlyCors = "DevelopmentOnlyCors";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<CourseDbContext>(options =>
            {
                var connectionString = new SqlConnectionStringBuilder
                {
                    DataSource = Configuration["Database:Source"],
                    InitialCatalog = Configuration["Database:Name"],
                    Password = Configuration["Database:Password"],
                    UserID = Configuration["Database:User"],
                    IntegratedSecurity = Configuration["Database:IntegratedSecurity"] == "true"
                }.ToString();

                options.UseSqlServer(connectionString);
            });

            services.AddHttpContextAccessor();

            services.AddIdentityCore<CourseUser>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<CourseDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<CourseUser, CourseDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddSpaStaticFiles(config =>
            {
                config.RootPath = "client-app/build";
            });

            services.AddCors(cors =>
            {
                cors.AddPolicy(DevelopmentOnlyCors, builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(DevelopmentOnlyCors);
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "client-app";

                if (env.IsDevelopment())
                {
                    spa.UseVueCliServer(npmScript: "serve");
                }
            });
        }
    }
}
