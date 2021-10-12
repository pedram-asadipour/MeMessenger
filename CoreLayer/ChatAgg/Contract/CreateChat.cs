using Microsoft.AspNetCore.Http;

namespace CoreLayer.ChatAgg.Contract
{
    public class CreateChat
    {
        public string Title { get; set; }
        public IFormFile Image { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsGroup { get; set; }
        public bool IsChannel { get; set; }

        public void Private()
        {
            IsPrivate = true;
        }

        public void Group(string title, IFormFile image)
        {
            Title = title;
            Image = image;
            IsGroup = true;
        }

        public void Channel(string title, IFormFile image)
        {
            Title = title;
            Image = image;
            IsChannel = true;
        }
    }
}