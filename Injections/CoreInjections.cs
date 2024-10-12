using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Solstice.API.Injections;

/// <summary>
/// The CoreInjections class is a static class containing methods related to core service injections in an ASP.NET Core application.
/// </summary>
public static class CoreInjections
{
    /// <summary>
    /// This extension method Configures the URL routing system to generate and recognize URLs in lowercase.
    /// </summary>
    /// <param name="services">The instance of IServiceCollection to add the service to.</param>
    public static void UseLowercaseRoutes(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
    }
}