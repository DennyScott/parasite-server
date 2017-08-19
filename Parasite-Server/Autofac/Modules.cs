using System;
using Autofac;
using Hive.Players.Autofac;

namespace Hive.Core.Autofac
{
    public class Modules
    {
        private static IContainer _container;

        public IContainer Init() 
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new PlayerRegistrationModule());

            _container = builder.Build();

            return _container;
        }
    }
}
