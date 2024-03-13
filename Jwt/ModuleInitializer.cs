using Commons;
using Microsoft.Extensions.DependencyInjection;

namespace Jwt;

class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
    }
}
