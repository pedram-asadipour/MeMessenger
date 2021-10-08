namespace _Framework.Auth
{
    public interface IAuthHelper
    {
        void Signin(AuthViewModel command);
        void Signout();
        bool IsAuth();
        AuthViewModel GetAuthAccount();
    }

    public class AuthViewModel
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public bool RememberMe { get; set; }

        public AuthViewModel()
        {
        }

        public AuthViewModel(long id, string username)
        {
            Id = id;
            Username = username;
        }
    }
}
