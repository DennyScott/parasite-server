using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Hive.Players.Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hive.Core
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
        }
		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule(new PlayerRegistrationModule());
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseWebSockets();

            TcpServer.StartServer(5678);
            TcpServer.Listen(); //Start Listening

			app.Use(async (context, next) =>
			{
				if (context.Request.Path == "/ws")
				{
					if (context.WebSockets.IsWebSocketRequest)
					{
						WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
						await Echo(context, webSocket);
					}
					else
					{
						context.Response.StatusCode = 400;
					}
				}
				else
				{
					await next();
				}

			});
        }

		private async Task Echo(Microsoft.AspNetCore.Http.HttpContext context, WebSocket webSocket)
		{
            Console.WriteLine("We are testing here!");
			var buffer = new byte[1024 * 4];
			WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
			while (!result.CloseStatus.HasValue)
			{
                Console.WriteLine(System.Text.Encoding.UTF8.GetString(buffer, 0 ,result.Count));
				await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
				result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

			}
			await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
		}
    }
}
