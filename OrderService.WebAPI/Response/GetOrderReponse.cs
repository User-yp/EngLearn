namespace OrderService.WebAPI.Response;

public record GetOrderReponse(string? TableName, string ProductionOrderNumber, string? ItemNumber, string ProjectText, int? Quantity,
    string Unit, string ProjectCategory, string Process, string PurchaseGroup, string ItemGroup, int? DeliveryTime, 
    DateTime DeliveryDate, int ProcessTime, bool IsComplete, DateTime CreationTime, DateTime? DeletionTime, DateTime? LastModificationTime);
