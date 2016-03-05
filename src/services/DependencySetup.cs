using System;
using Kcsara.Database.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kcsara.Database.Services
{
  public static class DependencySetup
  {
    public static void Go(IServiceCollection services, IConfiguration configuration)
    {
      // Add application services.
      services.AddTransient<IEmailSender, AuthMessageSender>();
      services.AddTransient<ISmsSender, AuthMessageSender>();

      services.AddSingleton<Func<IDataContext>>(svc => () => new DataContext(configuration["Data:DataStore:ConnectionString"]));

      services.AddSingleton<IMembersService, MembersService>();
    }
  }
}
