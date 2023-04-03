namespace _Framework.Auth
{
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
