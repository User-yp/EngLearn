using EventBus;
using IdentityService.Domain;
using IdentityService.Infrastructure;
using IdentityService.WebAPI.Events;
using IdentityService.WebAPI.Request;
using IdentityService.WebAPI.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.WebAPI.Controllers;

[Route("[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UserAdminController : ControllerBase
{
    private readonly IdUserManager userManager;
    private readonly IIdRepository repository;
    private readonly IEventBus eventBus;

    public UserAdminController(IdUserManager userManager, IEventBus eventBus, IIdRepository repository)
    {
        this.userManager = userManager;
        this.eventBus = eventBus;
        this.repository = repository;
    }

    [HttpGet]
    public Task<UserDTO[]> FindAllUsers()
    {
        return userManager.Users.Select(u => UserDTO.Create(u)).ToArrayAsync();
    }
    [HttpPost]
    [AllowAnonymous]
    public ActionResult< List<FindUsersBySearchResponse>> FindUsersBySearch(FindUsersBySearchRequest req)
    {
        var userList= repository.FindBySearchNameAsync(req.UserName);
        if (userList == null)
            return BadRequest("no user");
        var res = new List<FindUsersBySearchResponse>();
        foreach (var itme in userList)
        {
            var user = new FindUsersBySearchResponse(itme.UserName, itme.CreationTime, itme.PhoneNumber);
            res.Add(user);
        }
        return Ok(res);
    }
    [HttpGet]
    [AllowAnonymous]
    public ActionResult<List<FindUsersBySearchResponse>> FindAllLockedUsers()
    {
        var userList = repository.FindAllLockedUsers();
        if (userList == null)
            return BadRequest("no user");
        var res = new List<FindUsersBySearchResponse>();
        foreach (var itme in userList)
        {
            var user = new FindUsersBySearchResponse(itme.UserName, itme.CreationTime, itme.PhoneNumber);
            res.Add(user);
        }
        return Ok(res);
    }
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> ResetLockedUserByName(FindUsersBySearchRequest req)
    {
        (var res,var resbool) =await repository.ResetLockedUserByName(req.UserName);
        if (resbool)
            return Ok(res);
        return BadRequest("error");
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<UserDTO> FindById(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        return UserDTO.Create(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> AddAdminUser(AddAdminUserRequest req)
    {
        (var result, var user, var password) = await repository
            .AddAdminUserAsync(req.UserName, req.PhoneNum);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.SumErrors());
        }
        var userCreatedEvent = new UserCreatedEvent(user.Id, req.UserName, password, req.PhoneNum);
        eventBus.Publish("IdentityService.User.Created", userCreatedEvent);
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteAdminUser(Guid id)
    {
        await repository.RemoveUserAsync(id);
        return Ok();
    }
    [HttpDelete]
    [Route("{userName}")]
    public async Task<ActionResult> DeleteUser(string userName)
    {
        var res= await repository.RemoveUserByNameAsync(userName);
        return Ok(res);
    }
    [HttpPut]
    [AllowAnonymous]
    public async Task<ActionResult> ModifyUserInforById(ModifyUserInforByIdRequset req)
    {
        var res= await repository.ModifyUserInforByIdAsync(req.UserId, req.UserName, req.PhoneNumber, req.Password);
        return Ok(res);
    }
    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> UpdateAdminUser(Guid id, EditAdminUserRequest req)
    {
        var user = await repository.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound("用户没找到");
        }
        user.PhoneNumber = req.PhoneNum;
        await userManager.UpdateAsync(user);
        return Ok();
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<ActionResult> ResetAdminUserPassword(Guid id)
    {
        (var result, var user, var password) = await repository.ResetPasswordAsync(id);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.SumErrors());
        }
        //生成的密码短信发给对方
        var eventData = new ResetPasswordEvent(user.Id, user.UserName, password, user.PhoneNumber);
        eventBus.Publish("IdentityService.User.PasswordReset", eventData);
        return Ok();
    }
}
