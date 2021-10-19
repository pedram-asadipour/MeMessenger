using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using _Framework;
using Microsoft.AspNetCore.Http;

namespace CoreLayer.ChatAgg.Contract
{
    public class CreateChat
    {
        [Required(ErrorMessage = OperationMessage.Required)]
        [DisplayName("عنوان")]
        public string Title { get; set; }

        [Required(ErrorMessage = OperationMessage.Required)]
        [FileExtensionLimit(new []{ ".jpg", ".jpeg", ".png", ".gif", },ErrorMessage = OperationMessage.ImageExtension + " : jpg,jpeg,pnd,gif")]
        [DisplayName("تصویر")]
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