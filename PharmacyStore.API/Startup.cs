using BusinessLogic.Mapper;
using BusinessLogic.Repository;
using BusinessLogic.Repository.IRepository;
using DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PharmacyStore.API.Data;
using PharmacyStore.API.Services;
using PharmacyStore.API.Services.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PharmacyStore.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>().
                AddEntityFrameworkStores<ApplicationDbContext>().
                AddDefaultTokenProviders();

            services.AddCors(x => x.AddPolicy("PharmacyStore.API", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSwaggerGen(x => { //Include Swagger
                x.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "Pharmacy Store API", 
                    Version = "v1"
                });

                //In Swagger we can make any comments from <summary> section to controllers.
                var xfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; //xml file. Need for PharmacyStore.API/Properties/Build/XML documentation file.
                var xpath = Path.Combine(AppContext.BaseDirectory, xfile);

                x.IncludeXmlComments(xpath);
            });

            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(MappingProfile));

            //JWTbearer.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<SeedData>();

            // NLog services, need for logging.
            services.AddSingleton<ILoggerService, LoggerService>();

            // Added AddControllers() and removed AddRazorPages(). Because we don't need view in API.
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
                .AddNewtonsoftJson(options => {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SeedData dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pharmacy Store API v1");
                    c.RoutePrefix = "";
                });
            }

            app.UseHttpsRedirection();
            //app.UseStaticFiles(); Removed UseStaticFiles() and wwwroot folder.

            app.UseCors("PharmacyStore.API");

            //SeedData.Seed(userManager, roleManager).Wait();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            dbInitializer.Initialize(); // Seed admin user and another roles to database.

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapControllers(); // Removed MapRazorPages() and added MapControllers().
            });
        }
    }
}
