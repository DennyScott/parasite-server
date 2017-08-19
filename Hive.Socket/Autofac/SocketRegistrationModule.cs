using System;
using Autofac;
using Hive.Socket.Components;

namespace Hive.Socket.Autofac
{
    public class SocketRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Messaging>()
                   .AsImplementedInterfaces();

            builder.RegisterType<Connect>()
                   .AsImplementedInterfaces();

            builder.RegisterType<TcpServer>()
                   .AsImplementedInterfaces()
                   .SingleInstance()
                   .AutoActivate();
        }
    }
}
