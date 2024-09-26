namespace eBid.Identity.API.Services;

public class EFSignUpService : ISignUpService<ApplicationUser>
{
    private UserManager<ApplicationUser> _userManager;

    public EFSignUpService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public Task SignUp(ApplicationUser user)
    {
        return _userManager.CreateAsync(user);
    }
}