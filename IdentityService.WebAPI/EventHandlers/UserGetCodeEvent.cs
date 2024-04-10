namespace IdentityService.WebAPI.Events;

public record UserGetCodeEvent( string PhoneNum, int Code);
