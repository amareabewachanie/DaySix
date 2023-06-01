using DaySixMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DaySixMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var login = new LoginModel()
            {
                
                ReturnUrl = returnUrl,
            };
            return View(login);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user=await _userManager.FindByEmailAsync(loginModel.Email);
                if(user is not null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult signInResult =await _signInManager.PasswordSignInAsync(user,loginModel.Password,false,false);
                    if (signInResult.Succeeded)
                        return Redirect(loginModel.ReturnUrl ?? "/");
                    ModelState.AddModelError(nameof(loginModel.Password), "Login Failed:Incorrect Password");
                }
                ModelState.AddModelError(nameof(loginModel.Email), "Login Failed:Inavlid Email or Not Registered");
            }
            return View(loginModel);
        }
        [HttpGet]
        public IActionResult Register()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            bool cpasswordConfirmd = model.Password == model.ConfirmPasword;
            if (!cpasswordConfirmd) {
                ModelState.AddModelError("ConfirmPassword", "Please Confirm Your Password");
             }
             
            if (ModelState.IsValid)
            {
                AppUser user=new AppUser()
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName=model.Email
                };
                IdentityResult result=await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                   Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, model.Password,false,false);
                    return Redirect("/");
                }
                else
                {
                    foreach(IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login",new LoginModel { ReturnUrl="/"});
        }
    }
}
