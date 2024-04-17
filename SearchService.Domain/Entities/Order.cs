namespace SearchService.Domain.Entities;

public record Order(Guid TableId, Guid OrderId, string TableName, string ProjectText);

