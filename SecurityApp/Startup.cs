using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SecurityApp.Controllers;
using SecurityApp.Security;

namespace SecurityApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Is16", builder =>
                {
                    builder.AddRequirements(new MinimumAgeRequirement(16));
                });
   
                options.AddPolicy("InvoiceReader", builder =>
                {
                    builder.RequireClaim("invoice", "read", "write");
                });
                options.AddPolicy("InvoiceWriter", builder =>
                {
                    builder.RequireClaim("invoice", "write");
                });

            });

            services.AddMvc();
            services.AddScoped<IInvoiceService, InvoiceService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseStaticFiles();


            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "MyCookies",
                CookieHttpOnly = true,
                LoginPath = new PathString("/Security/Login"),
                AccessDeniedPath = new PathString("/Security/Nope"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "External",
                CookieHttpOnly = true,
                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });


            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions()
                {
                    AuthenticationScheme = "Google",
                    SignInScheme = "External",
                    Authority = "https://accounts.google.com/",
                    ClientId = "906661353041-3671qbksstne9jdue54j10d1n83tejjs.apps.googleusercontent.com",
                    ClientSecret = "GeA_0EQiOKrQ469CQmjdJ2It",
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        NameClaimType = "name"
                    },
                    AutomaticAuthenticate = false,
                    AutomaticChallenge = false
                }
            );
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
