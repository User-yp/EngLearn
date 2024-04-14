using Commons;
using Microsoft.Extensions.DependencyInjection;
using Order.Domain;
using Order.Domain.Entities;

namespace Order.Infrastructure;

public class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<IOrderDomainService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ITableRepository, TableRepository>();
    }
}
