using Commons;
using Listening.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Listening.Infrastructure;

class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<IListeningRepository, ListeningRepository>();
    }
}