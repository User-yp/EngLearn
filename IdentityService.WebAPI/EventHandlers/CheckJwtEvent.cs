using IdentityService.Domain.Entity;

namespace IdentityService.WebAPI.Events;

public record CheckJwtEvent(User? User);
