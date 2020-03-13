using Bunnypro.SpaService.VueCli;
using KnowledgeShare.Manager;
using KnowledgeShare.Manager.Validation.CourseValidators;
using KnowledgeShare.Server.Middlewares;
using KnowledgeShare.Store.Abstractions;
using KnowledgeShare.Store.Core;
using KnowledgeShare.Store.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

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
            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddRazorPages();

            services.AddDbContext<CourseContext>(options =>
            {
                var connectionString = new SqlConnectionStringBuilder
                {
                    DataSource = Configuration["Database:Server"],
                    Authentication = SqlAuthenticationMethod.SqlPassword,
                    InitialCatalog = Configuration["Database:Name"],
                    Password = Configuration["Database:Password"],
                    UserID = Configuration["Database:User"],
                    IntegratedSecurity = Configuration["Database:IntegratedSecurity"] == "true"
                }.ToString();

                options.UseSqlServer(connectionString);
            });

            services.AddHttpContextAccessor();

            services.AddScoped<IUserStore<CourseUser>, CourseUserStore<CourseContext>>();
            services.AddScoped<ICourseStore, CourseStore<CourseContext>>();

            services.AddScoped<ICourseValidator, DefaultCourseValidator>();

            services.AddScoped<CourseUserManager>();
            services.AddScoped<UserManager<CourseUser>>(service =>
                service.GetRequiredService<CourseUserManager>());
            services.AddScoped<CourseManager>();

            services.AddDefaultIdentity<CourseUser>()
                .AddEntityFrameworkStores<CourseContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<CourseUser, CourseContext>();

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
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
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

            // app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
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
