using System.ComponentModel.DataAnnotations;

namespace P.PresentationLayer.ViewModels
{
	public class ForgetPaswwordViewModel
	{
		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }
	}
}
