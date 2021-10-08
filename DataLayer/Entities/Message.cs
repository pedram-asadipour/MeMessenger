namespace DataLayer.Entities
{
    public class Message : BaseEntity
    {
        public long ChatId { get; private set; }
        public long AccountId { get; private set; }
        public string Body { get; private set; }
        public bool IsFile { get; private set; }

        public Chat Chat { get; private set; }
        public Account Account { get; private set; }

        protected Message()
        {
        }

        public Message(long chatId, long accountId, string body, bool isFile)
        {
            ChatId = chatId;
            AccountId = accountId;
            Body = body;
            IsFile = isFile;
        }
    }
}