using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class Chat : BaseEntity
    {
        public string Title { get; private set; }
        public bool IsPrivate { get; private set; }
        public bool IsGroup { get; private set; }
        public bool IsChannel { get; private set; }

        public IEnumerable<Message> Messages { get; private set; }
        public IEnumerable<UserChat> UserChats { get; private set; }

        protected Chat()
        {
        }

        public void Private()
        {
            Title = "";
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