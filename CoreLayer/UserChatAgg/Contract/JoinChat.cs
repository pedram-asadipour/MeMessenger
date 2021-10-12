namespace CoreLayer.UserChatAgg.Contract
{
    public class JoinChat
    {
        public long ChatId { get; set; }
        public long AccountId { get; set; }
        public int Permission { get; set; }

        public JoinChat()
        {
        }

        public JoinChat(long chatId, long accountId, int permission)
        {
            ChatId = chatId;
            AccountId = accountId;
            Permission = permission;
        }
    }
}