using _Framework;

namespace CoreLayer.AccountAgg.Contract
{
    public interface IAccountServices
    {
        OperationResult Signin(SigninViewModel command);
        OperationResult Signup(SignupViewModel command);
        OperationResult Signout();
    }
}