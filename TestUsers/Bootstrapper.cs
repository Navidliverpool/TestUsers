using System.Web.Mvc;
using Microsoft.Practices.Unity;
using TestUsers.Data.Repositories;
using Unity.Mvc4;

namespace TestUsers
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();    
            container.RegisterType<IMotorcycleRepository, MotorcycleRepository>();
            container.RegisterType<IBrandRepository, BrandRepository>();
            container.RegisterType<IDealerRepository, DealerRepository>();
            container.RegisterType<ICategoryRepository, CategoryRepository>();

            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}