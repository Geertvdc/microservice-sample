using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Akka.Actor;
using shoppingcartapi.Actors;
using Akka.Configuration;

namespace shoppingcartapi
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
            var config = ConfigurationFactory.ParseString(@"
            akka.persistence {
                journal {
                    plugin = ""akka.persistence.journal.sql-server""
                    sql-server {
                        class = ""Akka.Persistence.SqlServer.Journal.SqlServerJournal, Akka.Persistence.SqlServer""
                        schema-name = dbo
                        auto-initialize = on
                        connection-string = ""Server=tcp:cart-akka.database.windows.net,1433;Initial Catalog=cart-eventstore;Persist Security Info=False;User ID=akkaadmin;Password=Akka123456;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;""
                    }
                }
            }");
            
            services.AddSingleton<ActorSystem>(_ => ActorSystem.Create("cartservice",config));
            services.AddSingleton<CartCoordinatorProvider>();

            services.AddMvc();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
