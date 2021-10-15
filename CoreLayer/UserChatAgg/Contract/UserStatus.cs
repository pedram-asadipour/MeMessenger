namespace CoreLayer.UserChatAgg.Contract
{
    public class UserStatus
    {
        public long Id { get; set; }
        public string ConnectionId { get; set; }
        public string Username { get; set; }
        public bool IsOnline { get; set; }
    }
}