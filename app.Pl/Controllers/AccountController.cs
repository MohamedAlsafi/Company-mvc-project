using app.DAL.model;
using app.Pl.Helper;
using app.Pl.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace app.Pl.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        //private readonly EmailSetting _emailSetting;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_emailSetting = emailSetting;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    FName = model.FName,
                    LName = model.LName,
                    IsAgree = model.IsAgree

                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                    foreach (var Erros in result.Errors)
                        ModelState.AddModelError(string.Empty, Erros.Description);

            }

            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var check = await _userManager.CheckPasswordAsync(user, model.Password);

                    if (check)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError(string.Empty, " Incorrect Password ");
                }
                else
                    ModelState.AddModelError(string.Empty, "Email is not Exists");


            }

            return View(model);
        }

        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                    var resetPasswordLink = Url.Action(
                        "ResetPassword",
                        "Account",
                        new { email = user.Email, token = tokenEncoded },
                        Request.Scheme
                    );

                    var emailMessage = new Email
                    {
                        Subject = "Reset Password",
                        To = model.Email,
                        Body = resetPasswordLink
                    };

                    EmailSetting.SendEmail(emailMessage);

                    return RedirectToAction(nameof(CheckYourInbox));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email does not exist");
                }
            }


            return View("ForgetPassword", model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }

        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var user = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (result.Succeeded)

                    return RedirectToAction(nameof(Login));

                else

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

            }
            return View(model);
        }
    }
}
