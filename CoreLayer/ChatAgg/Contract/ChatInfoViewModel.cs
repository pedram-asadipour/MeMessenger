namespace CoreLayer.ChatAgg.Contract
{
    public class ChatInfoViewModel
    {
        public long AccountId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public bool IsOnline { get; set; }
        public bool IsGroupOrChannel { get; set; }
    }
}