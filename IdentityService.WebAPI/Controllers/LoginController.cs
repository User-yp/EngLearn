using IdentityService.Domain.Entity;
using IdentityService.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using Tea.Utils;
using IdentityService.WebAPI.Request;
using IdentityService.WebAPI.Response;
using EventBus;
using IdentityService.WebAPI.Events;
using ASPNETCore.RedisService;
using System;
using OfficeOpenXml;

namespace IdentityService.WebAPI.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IIdRepository repository;
    private readonly IEventBus eventBus;
    private readonly IRedisHelper redis;
    private readonly IdDomainService idService;

    public LoginController(IdDomainService idService, IIdRepository repository,IEventBus eventBus,IRedisHelper redis)
    {
        this.idService = idService;
        this.repository = repository;
        this.eventBus = eventBus;
        this.redis = redis;
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> SignUpByPhoneAndPwd(SignUpByPhoneAndPwdRequset req)
    {
        (var verify, var verifyRes) = await repository.VerifySmsCodeAsync(req.PhoneNumber, req.Smscode);
        if (!verify)
            return BadRequest(verifyRes.Data);
        (var res,var user) =await repository.AddUserAsync(req.UserName, req.PhoneNumber, req.Password);
        
        if (!res.Succeeded)
            return BadRequest(res.Errors.SumErrors());
        
        await LoginByPhoneAndSmsCodeAsync(new LoginByPhoneAndSmsCodeRequest(req.PhoneNumber, req.Smscode));
        eventBus.Publish("IdentityService.User.SmsCodeSignUp", new UserSmsCodeSignUpEvent(user,req.PhoneNumber, req.Smscode));
        return Ok($"{verifyRes.Data}，注册成功！");
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<UserResponse>> GetUserInfo()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await repository.FindByIdAsync(Guid.Parse(userId));
        if (user == null)//可能用户注销了
            return NotFound();
        //出于安全考虑，不要机密信息传递到客户端
        //除非确认没问题，否则尽量不要直接把实体类对象返回给前端
        return new UserResponse(user.Id, user.PhoneNumber, user.CreationTime);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<string?>> LoginByPhoneAndPwd(LoginByPhoneAndPwdRequest req)
    {
        //todo：要通过行为验证码、图形验证码等形式来防止暴力破解
        (var checkResult, string? token,var jwtCheckRes) = await idService.LoginByPhoneAndPwdAsync(req.PhoneNum, req.Password);
        if (checkResult.Succeeded)
        {
            if(!jwtCheckRes)
                eventBus.Publish("IdentityService.CheckJwt", new CheckJwtEvent(await repository.FindByPhoneNumberAsync(req.PhoneNum)));
            return token;
        }
        else if (checkResult.IsLockedOut)
        {
            //尝试登录次数太多
            return StatusCode((int)HttpStatusCode.Locked, "此账号已经锁定");
        }
        else
        {
            string msg = "登录失败";
            return StatusCode((int)HttpStatusCode.BadRequest, msg);
        }
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<string>> LoginByUserNameAndPwd(LoginByUserNameAndPwdRequest req)
    {
        (var checkResult, var token, var jwtCheckRes) = await idService.LoginByUserNameAndPwdAsync(req.UserName, req.Password);
        if (checkResult.Succeeded)
        {
            if (!jwtCheckRes)
                eventBus.Publish("IdentityService.CheckJwt", new CheckJwtEvent(await repository.FindByNameAsync(req.UserName)));
            return token!;
        }
        else if (checkResult.IsLockedOut)//尝试登录次数太多
            return StatusCode((int)HttpStatusCode.Locked, "用户已经被锁定");
        else
        {
            string msg = checkResult.ToString();
            return BadRequest("登录失败" + msg);
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<string>> LoginByPhoneAndSmsCodeAsync(LoginByPhoneAndSmsCodeRequest req)
    {
        (var checkResult, var token,var jwtCheckRes) = await idService.LoginByPhoneAndSmsCodeAsync(req.PhoneNum, req.SmsCode);
        
        if (checkResult.Succeeded)
        {
            if (!jwtCheckRes)
                eventBus.Publish("IdentityService.CheckJwt", new CheckJwtEvent(await repository.FindByPhoneNumberAsync(req.PhoneNum)));
            await redis.KeyDeleteAsync(req.PhoneNum);
            return token!;
        }
        else if (checkResult.IsLockedOut)//尝试登录次数太多
            return StatusCode((int)HttpStatusCode.Locked, "用户已经被锁定");
        else
        {
            string msg = checkResult.ToString();
            return BadRequest("登录失败" + msg);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> ChangeUserPassword(ChangePasswordRequest req)
    {
        Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var res = await repository.ChangePasswordAsync(userId, req.Password,req.NewPassword);
        if (res.Succeeded)
            return Ok(res);
        else
            return BadRequest(res.Errors.SumErrors());
    }
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> RetrievePassword(RetrievePasswordRequest req)
    {
        Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var res= await repository.RetrievePasswordAsync(userId, req.Password,req.SmsCode);
        if(res.Succeeded)
            return Ok(res);
        else
            return BadRequest(res.Errors.SumErrors());

    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> UserModifyInfor(UserModifyInforRequset req)
    {
        Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var res = await repository.UserModifyInforByIdAsync(userId, req.UserName, req.PhoneNumber, req.PasswordRequest.Password, req.PasswordRequest.NewPassword);
        return Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<UserGetCodeResponse>> GetCode(SendCodeByPhoneRequest req)
    {
        var randomNumber = Convert.ToInt32(RandomExtensions.NextDouble(new Random(), 100000, 999999));

        await idService.SendCodeAsync(req.PhoneNumber, randomNumber.ToSafeString());
        var resp = new UserGetCodeResponse(req.PhoneNumber, randomNumber,DateTime.Now);
        eventBus.Publish("IdentityService.GetCode", new UserGetCodeEvent(req.PhoneNumber, randomNumber));
        return Ok(resp);
    }
}
