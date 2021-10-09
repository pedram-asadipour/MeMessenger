namespace CoreLayer.ChatAgg.Contract
{
    public class ChatViewModel
    {
        public long ChatId { get; set; }
        public long AccountId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageDate { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsGroup { get; set; }
        public bool IsChannel { get; set; }
        public bool IsOnline { get; set; }
    }
}