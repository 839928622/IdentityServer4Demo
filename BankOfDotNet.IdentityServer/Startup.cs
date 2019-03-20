using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankOfDotNet.IdentityServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings", false)
                .Build();//access to appSettings.json
            string connectionString = config.GetSection("connectionString").Value;
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;




            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                //.AddInMemoryIdentityResources(Config.GetIdentityResources())
                //.AddInMemoryApiResources(Config.GetAllApiResources())
                //.AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers())
            //configuration store :clients and resources
              .AddConfigurationStore(options =>
              {
                  options.ConfigureDbContext = db => db.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssembly));
              })

              //operational store:tokens,constents,codes,etc
              .AddOperationalStore(options =>
              {
                  options.ConfigureDbContext = db => db.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssembly));
              });
                
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

        }
    }
}
