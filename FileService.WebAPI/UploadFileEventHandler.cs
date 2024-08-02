using ASPNETCore;
using EventBus;
using FileService.Domain;
using FileService.Infrastructure;
using Newtonsoft.Json;

namespace FileService.WebAPI;
[UnitOfWork(typeof(FSDbContext))]
[EventName("FileService.UploadFile")]
public class UploadFileEventHandler : DynamicIntegrationEventHandler
{
    private readonly FSDbContext dbContext;
    private readonly FSDomainService service;

    public UploadFileEventHandler(FSDbContext dbContext, FSDomainService service)
    {
        this.dbContext = dbContext;
        this.service = service;
    }
    public override async Task HandleDynamic(string eventName, dynamic eventData)
    {
        CancellationToken can = default;
        var fileName = (string)eventData.FileName;
        var fileBytes = (byte[])eventData.File;

        using var memoryStream = new MemoryStream(fileBytes);
        var upItem = await service.HandlerUploadAsync(memoryStream, fileName, can);
        dbContext.Add(upItem);
        await dbContext.SaveChangesAsync();
    }
}
