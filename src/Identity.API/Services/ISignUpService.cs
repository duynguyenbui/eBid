namespace eBid.Identity.API.Services;

public interface ISignUpService<in T>
{
    Task SignUp(T user);
}