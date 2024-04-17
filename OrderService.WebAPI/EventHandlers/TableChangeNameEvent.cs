namespace OrderService.WebAPI.EventHandlers;

public record TableChangeNameEvent(Guid Id,string TableName);