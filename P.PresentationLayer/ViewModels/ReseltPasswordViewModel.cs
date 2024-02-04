using System.ComponentModel.DataAnnotations;

namespace P.PresentationLayer.ViewModels
{
    public class ReseltPasswordViewModel
    {
        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password Doesn't Match ")]
        public string ConfirmPassword { get; set; }
    }
}
