using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using API.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>();

            // ===== Add Identity ========
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = "Fiver.Security.Bearer",
                        ValidAudience = "Fiver.Security.Bearer",
                        IssuerSigningKey = JwtSecurityKey.Create("aaaaaaaaaaaaaaaa")
                        //ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings  
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;
            });
            // ===== Add MVC ========
            services.AddMvc();
            
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //        .AddJwtBearer(options =>
            //        {
            //            options.TokenValidationParameters =
            //                 new TokenValidationParameters
            //                 {
            //                     ValidateIssuer = true,
            //                     ValidateAudience = true,
            //                     ValidateLifetime = true,
            //                     ValidateIssuerSigningKey = true,
            //
            //                     ValidIssuer = "Fiver.Security.Bearer",
            //                     ValidAudience = "Fiver.Security.Bearer",
            //                     IssuerSigningKey = JwtSecurityKey.Create("fiversecret ")
            //                 };
            //
            //            options.Events = new JwtBearerEvents
            //            {
            //                OnAuthenticationFailed = context =>
            //                {
            //                    Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
            //                    return Task.CompletedTask;
            //                },
            //                OnTokenValidated = context =>
            //                {
            //                    Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
            //                    return Task.CompletedTask;
            //                }
            //            };
            //        });
            //
            //services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext dbContext)
        {
            //app.UseAuthentication();

            //app.UseMvcWithDefaultRoute();
            
            //dbContext.Database.EnsureCreated();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // ===== Use Authentication ======
            app.UseAuthentication();
            app.UseMvc();

            // ===== Create tables ======
            dbContext.Database.EnsureCreated();
        }
    }
}
