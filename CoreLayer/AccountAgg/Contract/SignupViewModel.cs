using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using _Framework;

namespace CoreLayer.AccountAgg.Contract
{
    public class SignupViewModel
    {
        [Required(ErrorMessage = OperationMessage.Required)]
        [DisplayName("نام کاربری")]
        public string Username { get; set; }

        [Required(ErrorMessage = OperationMessage.Required)]
        [DisplayName("رمز عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = OperationMessage.Required)]
        [DisplayName("تکرار رمز عبور")]
        [Compare("Password",ErrorMessage = OperationMessage.PasswordNotCompare)]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }
    }
}