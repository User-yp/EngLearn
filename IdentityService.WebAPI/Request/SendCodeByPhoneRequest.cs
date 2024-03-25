using FluentValidation;

namespace IdentityService.WebAPI.Request;

public record SendCodeByPhoneRequest(string PhoneNumber);
public class SendCodeByPhoneRequestValidator : AbstractValidator<SendCodeByPhoneRequest>
{
    public SendCodeByPhoneRequestValidator()
    {
        RuleFor(e => e.PhoneNumber).NotNull().NotEmpty();
    }
}
