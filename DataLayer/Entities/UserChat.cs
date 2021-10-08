namespace DataLayer.Entities
{
    public class UserChat
    {
        public long Id { get; private set; }
        public long ChatId { get; private set; }
        public long AccountId { get; private set; }
        public int Permission { get; private set; }

        public Chat Chat { get; private set; }
        public Account Account { get; private set; }

        protected UserChat()
        {
        }

        public UserChat(long chatId, long accountId, int permission)
        {
            ChatId = chatId;
            AccountId = accountId;
            Permission = permission;
        }
    }
}