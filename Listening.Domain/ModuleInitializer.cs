using Commons;
using Microsoft.Extensions.DependencyInjection;

namespace Listening.Domain;

class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<ListeningDomainService>();
    }
}
