using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using _Framework;
using Microsoft.AspNetCore.Http;

namespace CoreLayer.AccountAgg.Contract
{
    public class ProfileViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = OperationMessage.Required)]
        [DisplayName("نام کاربری")]
        public string Username { get; set; }
        [DisplayName("تصویر پروفایل")]
        [FileExtensionLimit(new []{".jpg",".png",".gif", ".jpeg" },ErrorMessage = OperationMessage.ImageExtension + " : jpg,pnd,jpeg,gif")]
        public IFormFile ProfileImage { get; set; }
    }
}