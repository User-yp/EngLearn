using IdentityService.Domain.Entity;

namespace IdentityService.WebAPI.Events;

public record UserSmsCodeSignUpEvent(User User, string PhoneNumber,string SmsCode);
