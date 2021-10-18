using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using _Framework;

namespace CoreLayer.AccountAgg.Contract
{
    public class EditPassword
    {
        public long Id { get; set; }
        [DisplayName("رمز عبور فعلی")]
        [Required(ErrorMessage = OperationMessage.Required)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DisplayName("رمز عبور جدید")]
        [Required(ErrorMessage = OperationMessage.Required)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DisplayName("تکرار رمز عبور جدید")]
        [Required(ErrorMessage = OperationMessage.Required)]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage = OperationMessage.PasswordNotCompare)]
        public string RePassword { get; set; }
    }
}