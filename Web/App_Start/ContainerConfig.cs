using Autofac;
using Autofac.Integration.Mvc;
using MongoDB.Collections;
using MongoDB.UnitOfWorks;

namespace Web.App_Start
{
    public static class ContainerConfig
    {
        public static IContainer RegisterComponent()
        {
            var builder = new ContainerBuilder();
            // Register mvc controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Map interface with class
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

            // Auto mapping all Class in Collections namespace(folder) with IClass interface
            builder.RegisterAssemblyTypes(typeof(BaseCollection).Assembly)
                .Where(t => t.Namespace.Contains("Collections") && !t.IsAbstract)
                .AsImplementedInterfaces().InstancePerRequest();

            return builder.Build();
        }
    }
}