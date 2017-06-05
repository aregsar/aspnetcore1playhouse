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

namespace playhouse
{
	public class AppSettings
	{
        //Built in config
		//ASPNETCORE_ENVIRONMENT = Development

		public string APP_NAME { get; set; } = "test";
        public bool DEBUG { get; set; } = false;
	}

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.AddEnvironmentVariables()
				.Build();
			//var items = configuration.AsEnumerable();

			//string connStr = configuration.GetConnectionString("default");
			//bool debug = configuration.GetValue<bool>("DEBUG");
			//MyBuildSettings(configuration);


			services.AddOptions();

            services.Configure<AppSettings>(configuration);

			services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app
                              , IHostingEnvironment env
                              , ILoggerFactory loggerFactory
                              , IOptions<AppSettings> configSettings)
        {
            // <DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="1.0.0" />

            AppSettings settings = configSettings.Value;

            //TODO: setup logging in program file main method
			loggerFactory.AddConsole(LogLevel.Debug);
            loggerFactory.AddDebug(LogLevel.Debug);


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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
