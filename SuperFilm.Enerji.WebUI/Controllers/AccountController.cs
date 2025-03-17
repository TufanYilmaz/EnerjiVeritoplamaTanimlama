using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperFilm.Enerji.WebUI;
using SuperFilm.Enerji.WebUI.Services.Identity;
using SuperFilm.Enerji.WebUI.ViewModels.AccountViewModels;
using TanvirArjel.EFCore.GenericRepository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Services.Identity.User> _signInManager;
        private readonly UserManager<Services.Identity.User> _userManager;
        private readonly IQueryRepository _queryRepository;


        public AccountController(SignInManager<Services.Identity.User> signInManager,
            UserManager<Services.Identity.User> userManager, 
            IQueryRepository queryRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _queryRepository = queryRepository;
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
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    
                    ModelState.AddModelError("", "Kullanıcı Adı veya Parola Yanlış");
                    return View(model);
                }
            }
            return View(model);
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
                Services.Identity.User user = new Services.Identity.User()
                {
                    Description = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Kullanıcı Bulunamadı");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = model.Email });
                }
            }
            return View(model);
        }
        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel() { Email=username});
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ModelState.AddModelError("", "Birşeyler Yanlış Gitti");
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email Bulunamadı");
                return View(model);
            }
            var result = await _userManager.RemovePasswordAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                return RedirectToAction("Login", "Account");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }
        public async Task<IActionResult> Logut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Users(CancellationToken cancellationToken)
        {
            var res = await _queryRepository.GetAsync<User>(specification: null,cancellationToken);
            return View();
        }
    }
}
