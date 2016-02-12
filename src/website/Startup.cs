﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using website.Models;
using website.Services;

namespace website
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      // Set up configuration sources.
      var builder = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .AddJsonFile("appsettings.local.json", optional: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

      if (env.IsDevelopment())
      {
        // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
        builder.AddUserSecrets();
      }

      builder.AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // Add framework services.
      services.AddEntityFramework()
          .AddSqlServer()
          .AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

      services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();

      services.AddMvc();

      // Add application services.
      services.AddTransient<IEmailSender, AuthMessageSender>();
      services.AddTransient<ISmsSender, AuthMessageSender>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment())
      {
        app.UseBrowserLink();
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");

        // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
        try
        {
          using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
              .CreateScope())
          {
            serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                 .Database.Migrate();
          }
        }
        catch { }
      }

      app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

      app.UseStaticFiles();

      app.UseIdentity();

      SetupExternalAuth(app, Configuration);

      app.UseMvc(routes =>
            {
              routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
            });
    }

    private void SetupExternalAuth(IApplicationBuilder app, IConfiguration configuration)
    {
      var authConfig = configuration.GetSection("externalAuth");
      if (authConfig == null) return;
      var order = authConfig.Get<string[]>("order");
      if (order == null) return;

      foreach (var key in order)
      {
        var authInfo = authConfig.GetSection(key);
        if (authInfo == null) continue;

        var type = authInfo.Get<string>("type");
        if (type == "OpenId")
        {
          app.UseOpenIdConnectAuthentication(options =>
          {
            options.ClientId = authInfo.Get<string>("clientId");
            options.ClientSecret = authInfo.Get<string>("clientSecret");
            options.Authority = authInfo.Get<string>("authority");
            options.DisplayName = authInfo.Get<string>("name");
            options.AuthenticationScheme = options.DisplayName;
          });
        }
      }
    }

    // Entry point for the application.
    public static void Main(string[] args) => WebApplication.Run<Startup>(args);
  }
}
