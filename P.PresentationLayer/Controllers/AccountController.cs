using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using P.DataAccessLayer.Models;
using P.PresentationLayer.Helpers;
using P.PresentationLayer.ViewModels;
using System.Threading.Tasks;

namespace P.PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _UserManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _UserManager = userManager;
			_signInManager = signInManager;
		}
        #region Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    IsAgree = model.IsAgree,
                    FName = model.FName,
                    LName = model.LName,
                };
                var Result = await _UserManager.CreateAsync(User, model.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var error in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        #endregion
        #region Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _UserManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var Result = await _UserManager.CheckPasswordAsync(user, model.Password);
                    if (Result)
                    {
                        //Login 
                        var LoginResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (LoginResult.Succeeded)
                        {
                            return RedirectToAction("index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Password Is Incorrect");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email Is Not Exsits");
                }
            }
            return View(model);
        }
        #endregion
        #region Sign Out
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion
        #region ForgetPassword
        public IActionResult ForgetPassword()
        {
            return View();
        }
        public async Task<IActionResult> SendEmail(ForgetPaswwordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _UserManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var Token = await _UserManager.GeneratePasswordResetTokenAsync(user);
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new {email=user.Email,token=Token}, Request.Scheme);
                    var email = new Email()
                    {
                        Subject = "ResetPassword",
                        To = model.Email,
                        Body = ResetPasswordLink
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Email Is Not Exists");

            }

            return View("ForgetPassword", model);

        }
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        #endregion
        #region ResetPassword
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ReseltPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var user = await _UserManager.FindByEmailAsync(email);
                var Result = await _UserManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (Result.Succeeded)

                    return RedirectToAction(nameof(Login));


                foreach (var error in Result.Errors)

                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
    } 
    #endregion

}

