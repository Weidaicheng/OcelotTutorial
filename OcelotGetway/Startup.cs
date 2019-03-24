using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Administration;
using Ocelot.Cache;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace OcelotGetway
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
            services.AddMvc();

            void options(IdentityServerAuthenticationOptions o)
            {
                o.Authority = "http://localhost:6000";
                o.RequireHttpsMetadata = false;
                o.ApiName = "api1";
            }

            services
                .AddOcelot(new ConfigurationBuilder()
                    .AddJsonFile("configuration.json")
                    .Build())
                .AddConsul()
                .AddPolly()
                .AddCacheManager(x => x.WithDictionaryHandle())
                .AddAdministration("/administration", "secret");
//                .AddAdministration("/administration", options);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication("TestKey", options);

            services.AddSingleton<IOcelotCache<CachedResponse>, MyCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            await app.UseOcelot();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
