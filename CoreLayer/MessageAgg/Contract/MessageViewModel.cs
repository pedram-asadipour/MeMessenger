namespace CoreLayer.MessageAgg.Contract
{
    public class MessageViewModel
    {
        public long Id { get; set; }
        public string ChatName { get; set; }
        public long ChatId { get; set; }
        public bool IsOwner { get; set; }
        public string Username { get; set; }
        public string ProfileImage { get; set; }
        public string ChatImage { get; set; }
        public string Body { get; set; }
        public bool IsFile { get; set; }
        public string SendDate { get; set; }
    }
}