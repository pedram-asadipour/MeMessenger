using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class Account : BaseEntity
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string ProfileImage { get; private set; }

        public IEnumerable<Message> Messages { get; private set; }
        public IEnumerable<UserChat> UserChats { get; private set; }

        protected Account()
        {
        }

        public Account(string username, string password, string profileImage = "")
        {
            Username = username;
            Password = password;

            if (!string.IsNullOrWhiteSpace(profileImage))
                ProfileImage = profileImage;
        }

        public void Edit(string username, string password, string profileImage = "")
        {
            Username = username;
            Password = password;

            if (!string.IsNullOrWhiteSpace(profileImage))
                ProfileImage = profileImage;
        }
    }
}