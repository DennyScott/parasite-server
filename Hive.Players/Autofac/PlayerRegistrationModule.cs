using System;
using Autofac;
using Hive.Players.Components;

namespace Hive.Players.Autofac
{
    public class PlayerRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Player>()
                   .AsImplementedInterfaces();

            builder.RegisterType<PlayerList>()
                   .AsImplementedInterfaces();
        }
    }
}
