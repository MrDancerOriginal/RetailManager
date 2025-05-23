using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Portal.Authentication;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Model;

namespace Portal
{
  public class Program
  {
    public async static Task Main(string[] args)
    {
      var builder = WebAssemblyHostBuilder.CreateDefault(args);
      builder.RootComponents.Add<App>("#app");

      builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
      builder.Services.AddBlazoredLocalStorage();
      builder.Services.AddAuthorizationCore();
      builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

      builder.Services.AddSingleton<IAPIHelper, APIHelper>();
      builder.Services.AddSingleton<ILoggedInUserModel, LoggedInUserModel>();

      builder.Services.AddTransient<IProductEndpoint, ProductEndpoint>();
      builder.Services.AddTransient<IUserEndPoint, UserEndPoint>();
      builder.Services.AddTransient<ISaleEndpoint, SaleEndpoint>();

      builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

      await builder.Build().RunAsync();
    }
  }
}
