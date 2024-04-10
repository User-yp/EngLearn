namespace IdentityService.WebAPI.Response;

public record FindUsersBySearchResponse(string UserName,DateTime CreationTime,string PhoneNumber);