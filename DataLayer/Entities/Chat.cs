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

        public Chat()
        {
            Messages = new List<Message>();
            UserChats = new List<UserChat>();
        }

        public void Private()
        {
            Title = null;
            IsPrivate = true;
            IsGroup = false;
            IsChannel = false;
        }

        public void Group(string title)
        {
            Title = title;
            IsGroup = true;
            IsPrivate = false;
            IsChannel = false;
        }

        public void Channel(string title)
        {
            Title = title;
            IsChannel = true;
            IsGroup = false ;
            IsPrivate = false;
        }
    }
}