using Autofac;
using Hive.Players.Autofac;
using Hive.Socket;
using Hive.Socket.Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.Core
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }
		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule(new PlayerRegistrationModule());
            builder.RegisterModule(new SocketRegistrationModule());
            ApplicationContainer = builder.Build();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var appLifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            var tcpServer = ApplicationContainer.Resolve<TcpServer>();
            appLifetime.ApplicationStopping.Register(() => tcpServer.StopServer());
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}
