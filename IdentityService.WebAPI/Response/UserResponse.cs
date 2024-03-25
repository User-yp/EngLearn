namespace IdentityService.WebAPI.Response;

public record UserResponse(Guid Id, string PhoneNumber, DateTime CreationTime);
