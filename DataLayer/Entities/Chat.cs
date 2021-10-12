using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class Chat : BaseEntity
    {
        public string Title { get; private set; }
        public string Image { get; set; }
        public bool IsPrivate { get; private set; }
        public bool IsGroup { get; private set; }
        public bool IsChannel { get; private set; }

        public List<Message> Messages { get; private set; }
        public List<UserChat> UserChats { get; private set; }

        protected Chat()
        {
            Messages = new List<Message>();
            UserChats = new List<UserChat>();
        }

        public Chat(string title, string image, bool isPrivate, bool isGroup, bool isChannel)
        {
            Title = title;
            Image = image;
            IsPrivate = isPrivate;
            IsGroup = isGroup;
            IsChannel = isChannel;
        }
    }
}