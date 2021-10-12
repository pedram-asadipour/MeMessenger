using Microsoft.AspNetCore.Http;

namespace CoreLayer.MessageAgg.Contract
{
    public class SendMessage
    {
        public long ChatId { get; set; }
        public long ReceiveId { get; set; }
        public string Body { get; set; }
        public IFormFile File { get; set; }
    }
}