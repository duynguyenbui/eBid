namespace eBid.Bidding.API.Infrastructure.Services;

public interface IIdentityService
{
    string GetUserIdentity();

    string GetUserName();
}