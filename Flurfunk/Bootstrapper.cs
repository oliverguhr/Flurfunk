using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using Flurfunk.Data.Interface;
using Flurfunk.Data;
using System.Configuration;
using System.Web.Http;

namespace Flurfunk
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      // register dependency resolver for WebAPI RC
      GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();    
      
      RegisterTypes(container);

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
        container.RegisterType<IDatabase, Database>(new InjectionConstructor(ConfigurationManager.AppSettings["mongoDb"], ConfigurationManager.AppSettings["mongoDbName"]));
        container.RegisterType<IMessageService, MessageService>();
        container.RegisterType<IUserService, UserService>();
        container.RegisterType<IGroupService, GroupService>();    
    }
  }
}