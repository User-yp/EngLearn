using FluentValidation;

namespace IdentityService.WebAPI.Request;

public record RetrievePasswordRequest(string Password,string SmsCode);
public class RetrievePasswordValidator : AbstractValidator<RetrievePasswordRequest>
{
    public RetrievePasswordValidator()
    {
        RuleFor(e=>e.Password).NotNull().NotEmpty();
        RuleFor(e => e.SmsCode).NotNull().NotEmpty();

        /*RuleFor(user => user.Password)
            .NotEmpty().WithMessage("密码不能为空")
            .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$")
            .WithMessage("密码必须包含数字、大小写字母和至少一个特殊字符，且长度至少为8位");

        RuleFor(e => e.SmsCode).NotNull().NotEmpty()
            .WithMessage("验证码不能为空")
            .Matches(@"^\d{6}$").WithMessage("验证码必须是六位数字");*/
    }
}