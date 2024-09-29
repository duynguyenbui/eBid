namespace eBid.Auction.API.Services.Identity;

public class IdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
{
    public string GetUserIdentity() => httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;

    public string GetUserName() => httpContextAccessor.HttpContext?.User.Identity?.Name;
}