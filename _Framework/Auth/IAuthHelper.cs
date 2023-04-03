namespace _Framework.Auth
{
    public interface IAuthHelper
    {
        void Signin(AuthViewModel command);
        void Signout();
        bool IsAuth();
        AuthViewModel GetAuthAccount();
    }
}
