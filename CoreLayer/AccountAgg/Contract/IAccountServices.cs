using _Framework;

namespace CoreLayer.AccountAgg.Contract
{
    public interface IAccountServices
    {
        ProfileViewModel GetCurrentAccount();
        string GetProfileImage();
        OperationResult EditAccount(ProfileViewModel command);
        OperationResult EditPassword(EditPassword command);
        OperationResult Signin(SigninViewModel command);
        OperationResult Signup(SignupViewModel command);
        OperationResult Signout();
    }
}