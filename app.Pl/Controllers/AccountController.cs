using app.DAL.model;
using app.Pl.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace app.Pl.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager , SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                   var result =   await _signInManager.PasswordSignInAsync(user,model.Password, model.RememberMe,false);
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

        public new async Task <IActionResult> SignOut()
        {
          await  _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));   
        }
    }
}
