using System.Security.Claims;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IQueryRepository _queryRepository;


        public AccountController(SignInManager<Services.Identity.User> signInManager,
            UserManager<Services.Identity.User> userManager,
            RoleManager<IdentityRole> roleManager,
            IQueryRepository queryRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _queryRepository = queryRepository;
            _roleManager = roleManager;
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
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    HttpContext.Session.SetString("username", user!.Description);

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
            _roleManager.CreateAsync(new IdentityRole("SuperAdmin") { }).Wait();
            _roleManager.CreateAsync(new IdentityRole("Admin") { }).Wait();
            _roleManager.CreateAsync(new IdentityRole("User") { }).Wait();
            _roleManager.CreateAsync(new IdentityRole("Manager") { }).Wait();
            return View();
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Services.Identity.User user = new Services.Identity.User()
                {
                    Description = model.Name,
                    Email = model.Email,
                    UserName = model.Name,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

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
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User); 
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View(model);
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
                return RedirectToAction("Index", "Home");
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


        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new ProfileViewModel
            {
                Email = user.Email,
                Name = user.Description,
                Roles = roles.ToList()
            };

            return View(model);
        }



        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Users(CancellationToken cancellationToken)
        {
            var res = await _queryRepository.GetAsync<User>(specification: null, cancellationToken);
            return View();
        }
    }
}