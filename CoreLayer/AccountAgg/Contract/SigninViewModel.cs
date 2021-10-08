using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using _Framework;

namespace CoreLayer.AccountAgg.Contract
{
    public class SigninViewModel
    {
        [Required(ErrorMessage = OperationMessage.Required)]
        [DisplayName("نام کاربری")]
        public string Username { get; set; }

        [Required(ErrorMessage = OperationMessage.Required)]
        [DisplayName("رمز عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}