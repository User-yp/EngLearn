using FluentValidation;

namespace IdentityService.WebAPI.Request;

public record FindUsersBySearchRequest(string UserName);
public class FindUsersBySearchValidator : AbstractValidator<FindUsersBySearchRequest>
{
    public FindUsersBySearchValidator()
    {
        RuleFor(u=>u.UserName).NotNull().NotEmpty();
    }
}