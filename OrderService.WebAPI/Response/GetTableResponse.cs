namespace OrderService.WebAPI.Response;

public record GetTableResponse(string TableName, bool IsComplete, DateTime CreationTime, DateTime? LastModificationTime );