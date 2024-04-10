namespace IdentityService.WebAPI.Response;

public record UserGetCodeResponse(string PhoneNumber,int Code,DateTime DateTime);
